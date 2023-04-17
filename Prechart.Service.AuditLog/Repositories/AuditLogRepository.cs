using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Prechart.Service.AuditLog.Database.Context;
using Prechart.Service.AuditLog.Models;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Repositories;

public partial class AuditLogRepository : IAuditLogRepository
{
    private readonly IMemoryCache _cache;
    private readonly IAuditLogDbContext _context;
    private readonly IMongoCollection<ControllerLogs> _controllerLogs;
    private readonly ILogger<AuditLogRepository> _logger;
    private readonly IMongoCollection<MongoAuditLogs> _mongoAuditLogs;

    private readonly IAsyncPolicy _retryPolicy =
        Policy
            .Handle<MongoException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    public AuditLogRepository(ILogger<AuditLogRepository> logger,
        IAuditLogDbContext context,
        IMemoryCache cache,
        IMongoCollection<MongoAuditLogs> mongoAuditLogs,
        IMongoCollection<ControllerLogs> controllerLogs)
    {
        _logger = logger;
        _context = context;
        _cache = cache;
        _mongoAuditLogs = mongoAuditLogs;
        _controllerLogs = controllerLogs;
    }

    public async Task<IFluentResults<bool>> HandleAsync(InsertAuditLog request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AuditLogs is null)
            {
                return ResultsTo.NotFound<bool>();
            }

            if (request.AuditLogs.Any())
            {
                await _retryPolicy.ExecuteAsync(async () => await _mongoAuditLogs.InsertManyAsync(request.AuditLogs));
            }

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(LogControllerActions request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return ResultsTo.NotFound<bool>();
            }
            await _controllerLogs.InsertOneAsync(new ControllerLogs
            {
                Id = ObjectId.GenerateNewId(),
                User = request.ControllerLogs.User,
                ActionOn = request.ControllerLogs.ActionOn,
                Data = request.ControllerLogs.Data,
            });

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<MongoAuditLogs>>> HandleAsync(GetAuditTrail request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.TableName))
            {
                return ResultsTo.BadRequest<List<MongoAuditLogs>>();
            }

            var filters = Builders<MongoAuditLogs>.Filter.Where(p => p.Table.ToLower().Contains(request.TableName.ToLower()));

            var trace = await _mongoAuditLogs.Find(filters).ToListAsync();

            if ((request.TableId ?? 0) != 0)
            {
                trace = trace.Where(a => a.TableId == request.TableId).ToList();
            }

            return ResultsTo.Success(trace.OrderByDescending(logs => logs.LogDate).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<MongoAuditLogs>>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(InsertAuditLogInMemory request, CancellationToken cancellationToken)
    {
        try
        {

            if (request.UserName is null)
            {
                return ResultsTo.BadRequest<bool>().WithMessage("Username is null");
            }
            else if (request.TableName is null)
            {
                return ResultsTo.BadRequest<bool>().WithMessage("TableName is null");
            }
            else if (request.Action is null)
            {
                return ResultsTo.BadRequest<bool>().WithMessage("Action is null");
            }
            else
            {
                _context.AuditLog.Add(new Database.Models.AuditLog
                {
                    LogDate = DateTime.Now,
                    User = request.UserName,
                    Table = request.TableName,
                    TableId = request.EntityId,
                    Action = request.Action,
                    Changes = request.Changes?.ToJsonString(),
                    AdditionalLogs = request.AdditionalLogs,
                });

                await _context.SaveChangesAsync();
                return ResultsTo.Success(true);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<Database.Models.AuditLog>>> HandleAsync(GetAuditLogReadyForPosting request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.AuditLog.OrderBy(l => l.LogDate).ToListAsync();

            if (!result.Any())
            {
                return ResultsTo.NotFound<List<Database.Models.AuditLog>>();
            }

            _context.AuditLog.RemoveRange(result);

            await _context.SaveChangesAsync();

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<Database.Models.AuditLog>>().FromException(e);
        }
    }
}
