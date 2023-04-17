using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Belastingen.Database.Models;
using System;
using Prechart.Service.Belastingen.Database.Models.Berekeningen;

namespace Prechart.Service.Belastingen.Database.Context;

public class BelastingenDbContext : DbContext, IBelastingenDbContext
{
    private readonly ISaveDatabaseHelper _saveDatabaseHelper;

    public BelastingenDbContext()
    {
    }

    public BelastingenDbContext(DbContextOptions<BelastingenDbContext> options, ISaveDatabaseHelper saveDatabaseHelper)
        : this(options) => _saveDatabaseHelper = saveDatabaseHelper;

    public BelastingenDbContext(DbContextOptions<BelastingenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<White> White { get; set; }
    public virtual DbSet<Green> Green { get; set; }
    public virtual DbSet<Woonlandbeginsel> Woonlandbeginsel { get; set; }
    public virtual DbSet<Landen> Landen { get; set; }
    public virtual DbSet<AOW> AOW { get; set; }
    public virtual DbSet<PremiePercentagesSocialeVerzekeringen> PremiePercentagesSocialeVerzekeringen { get; set; }
    public virtual DbSet<Berekeningen> Berekeningen { get; set; }

    public async Task Init() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Woonlandbeginsel>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(p => new
            {
                p.Id,
                p.WoonlandbeginselCode,
            });
            entity.HasData(
                new Woonlandbeginsel { Id = 1, WoonlandbeginselCode = "NL", WoonlandbeginselBenaming = "Nederland", WoonlandbeginselBelastingCode = 2, Active = true },
                new Woonlandbeginsel { Id = 2, WoonlandbeginselCode = "BE", WoonlandbeginselBenaming = "België", WoonlandbeginselBelastingCode = 2, Active = true },
                new Woonlandbeginsel { Id = 3, WoonlandbeginselCode = "LK", WoonlandbeginselBenaming = "Landenkring", WoonlandbeginselBelastingCode = 2, Active = true },
                new Woonlandbeginsel { Id = 4, WoonlandbeginselCode = "DL", WoonlandbeginselBenaming = "Derde Landen", WoonlandbeginselBelastingCode = 2, Active = true },
                new Woonlandbeginsel { Id = 5, WoonlandbeginselCode = "SA", WoonlandbeginselBenaming = "Suriname/Aruba", WoonlandbeginselBelastingCode = 2, Active = true }
            );
        });

        modelBuilder.Entity<Landen>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(p => new
            {
                p.Id,
                p.LandenCode,
            });
            entity.HasData(
                new Landen { Id =  1, LandenCode = "AD", LandenNaam = "Andorra", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  2, LandenCode = "AE", LandenNaam = "Verenigde Arabische Emiraten", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  3, LandenCode = "AF", LandenNaam = "Afghanistan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  4, LandenCode = "AG", LandenNaam = "Antigua en Barbuda", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  5, LandenCode = "AI", LandenNaam = "Anguilla", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  6, LandenCode = "AL", LandenNaam = "Albanië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  7, LandenCode = "AM", LandenNaam = "Armenië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  8, LandenCode = "AO", LandenNaam = "Angola", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  9, LandenCode = "AQ", LandenNaam = "Antarctica", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  10, LandenCode = "AR", LandenNaam = "Argentinië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  11, LandenCode = "AS", LandenNaam = "Amerikaans-Samoa", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  12, LandenCode = "AT", LandenNaam = "Oostenrijk", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  13, LandenCode = "AU", LandenNaam = "Australië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  14, LandenCode = "AW", LandenNaam = "Aruba", WoonlandbeginselIndicatie = 4, Active = true },
                new Landen { Id =  15, LandenCode = "AX", LandenNaam = "Åland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  16, LandenCode = "AZ", LandenNaam = "Azerbeidzjan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  17, LandenCode = "BA", LandenNaam = "Bosnië en Herzegovina", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  18, LandenCode = "BB", LandenNaam = "Barbados", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  19, LandenCode = "BD", LandenNaam = "Bangladesh", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  20, LandenCode = "BE", LandenNaam = "België", WoonlandbeginselIndicatie = 3, Active = true },
                new Landen { Id =  21, LandenCode = "BF", LandenNaam = "Burkina Faso", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  22, LandenCode = "BG", LandenNaam = "Bulgarije", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  23, LandenCode = "BH", LandenNaam = "Bahrein", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  24, LandenCode = "BI", LandenNaam = "Burundi", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  25, LandenCode = "BJ", LandenNaam = "Benin", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  26, LandenCode = "BL", LandenNaam = "Saint-Barthélemy", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  27, LandenCode = "BM", LandenNaam = "Bermuda", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  28, LandenCode = "BN", LandenNaam = "Brunei", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  29, LandenCode = "BO", LandenNaam = "Bolivia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  30, LandenCode = "BQ", LandenNaam = "Caribisch Nederland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  31, LandenCode = "BR", LandenNaam = "Brazilië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  32, LandenCode = "BS", LandenNaam = "Bahamas", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  33, LandenCode = "BT", LandenNaam = "Bhutan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  34, LandenCode = "BV", LandenNaam = "Bouveteiland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  35, LandenCode = "BW", LandenNaam = "Botswana", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  36, LandenCode = "BY", LandenNaam = "Wit-Rusland (Belarus)", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  37, LandenCode = "BZ", LandenNaam = "Belize", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  38, LandenCode = "CA", LandenNaam = "Canada", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  39, LandenCode = "CC", LandenNaam = "Cocoseilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  40, LandenCode = "CD", LandenNaam = "Congo-Kinshasa", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  41, LandenCode = "CF", LandenNaam = "Centraal-Afrikaanse Republiek", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  42, LandenCode = "CG", LandenNaam = "Congo-Brazzaville", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  43, LandenCode = "CH", LandenNaam = "Zwitserland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  44, LandenCode = "CI", LandenNaam = "Ivoorkust", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  45, LandenCode = "CK", LandenNaam = "Cookeilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  46, LandenCode = "CL", LandenNaam = "Chili", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  47, LandenCode = "CM", LandenNaam = "Kameroen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  48, LandenCode = "CN", LandenNaam = "China", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  49, LandenCode = "CO", LandenNaam = "Colombia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  50, LandenCode = "CR", LandenNaam = "Costa Rica", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  51, LandenCode = "CU", LandenNaam = "Cuba", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  52, LandenCode = "CV", LandenNaam = "Kaapverdië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  53, LandenCode = "CW", LandenNaam = "Curaçao", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  54, LandenCode = "CX", LandenNaam = "Christmaseiland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  55, LandenCode = "CY", LandenNaam = "Cyprus", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  56, LandenCode = "CZ", LandenNaam = "Tsjechië", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  57, LandenCode = "DE", LandenNaam = "Duitsland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  58, LandenCode = "DJ", LandenNaam = "Djibouti", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  59, LandenCode = "DK", LandenNaam = "Denemarken", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  60, LandenCode = "DM", LandenNaam = "Dominica", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  61, LandenCode = "DO", LandenNaam = "Dominicaanse Republiek", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  62, LandenCode = "DZ", LandenNaam = "Algerije", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  63, LandenCode = "EC", LandenNaam = "Ecuador", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  64, LandenCode = "EE", LandenNaam = "Estland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  65, LandenCode = "EG", LandenNaam = "Egypte", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  66, LandenCode = "EH", LandenNaam = "Westelijke Sahara", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  67, LandenCode = "ER", LandenNaam = "Eritrea", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  68, LandenCode = "ES", LandenNaam = "Spanje", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  69, LandenCode = "ET", LandenNaam = "Ethiopië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  70, LandenCode = "FI", LandenNaam = "Finland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  71, LandenCode = "FJ", LandenNaam = "Fiji", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  72, LandenCode = "FK", LandenNaam = "Falklandeilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  73, LandenCode = "FM", LandenNaam = "Micronesia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  74, LandenCode = "FO", LandenNaam = "Faeröer", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  75, LandenCode = "FR", LandenNaam = "Frankrijk", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  76, LandenCode = "GA", LandenNaam = "Gabon", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  77, LandenCode = "GB", LandenNaam = "Verenigd Koninkrijk", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  78, LandenCode = "GD", LandenNaam = "Grenada", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  79, LandenCode = "GE", LandenNaam = "Georgië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  80, LandenCode = "GF", LandenNaam = "Frans-Guyana", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  81, LandenCode = "GG", LandenNaam = "Guernsey", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  82, LandenCode = "GH", LandenNaam = "Ghana", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  83, LandenCode = "GI", LandenNaam = "Gibraltar", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  84, LandenCode = "GL", LandenNaam = "Groenland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  85, LandenCode = "GM", LandenNaam = "Gambia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  86, LandenCode = "GN", LandenNaam = "Guinee", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  87, LandenCode = "GP", LandenNaam = "Guadeloupe", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  88, LandenCode = "GQ", LandenNaam = "Equatoriaal-Guinea", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  89, LandenCode = "GR", LandenNaam = "Griekenland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  90, LandenCode = "GS", LandenNaam = "Zuid-Georgia en de Zuidelijke Sandwicheilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  91, LandenCode = "GT", LandenNaam = "Guatemala", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  92, LandenCode = "GU", LandenNaam = "Guam", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  93, LandenCode = "GW", LandenNaam = "Guinee-Bissau", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  94, LandenCode = "GY", LandenNaam = "Guyana", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  95, LandenCode = "HK", LandenNaam = "Hongkong", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  96, LandenCode = "HM", LandenNaam = "Heard en McDonaldeilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  97, LandenCode = "HN", LandenNaam = "Honduras", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  98, LandenCode = "HR", LandenNaam = "Kroatië", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  99, LandenCode = "HT", LandenNaam = "Haïti", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  100, LandenCode = "HU", LandenNaam = "Hongarije", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  101, LandenCode = "ID", LandenNaam = "Indonesië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  102, LandenCode = "IE", LandenNaam = "Ierland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  103, LandenCode = "IL", LandenNaam = "Israël", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  104, LandenCode = "IM", LandenNaam = "Man (Eiland)", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  105, LandenCode = "IN", LandenNaam = "India", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  106, LandenCode = "IO", LandenNaam = "Brits Indische Oceaanterritorium", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  107, LandenCode = "IQ", LandenNaam = "Irak", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  108, LandenCode = "IR", LandenNaam = "Iran", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  109, LandenCode = "IS", LandenNaam = "IJsland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  110, LandenCode = "IT", LandenNaam = "Italië", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  111, LandenCode = "JE", LandenNaam = "Jersey", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  112, LandenCode = "JM", LandenNaam = "Jamaica", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  113, LandenCode = "JO", LandenNaam = "Jordanië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  114, LandenCode = "JP", LandenNaam = "Japan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  115, LandenCode = "KE", LandenNaam = "Kenia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  116, LandenCode = "KG", LandenNaam = "Kirgizië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  117, LandenCode = "KH", LandenNaam = "Cambodja", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  118, LandenCode = "KI", LandenNaam = "Kiribati", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  119, LandenCode = "KM", LandenNaam = "Comoren", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  120, LandenCode = "KN", LandenNaam = "Saint Kitts en Nevis", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  121, LandenCode = "KP", LandenNaam = "Noord-Korea", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  122, LandenCode = "KR", LandenNaam = "Zuid-Korea", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  123, LandenCode = "KW", LandenNaam = "Koeweit", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  124, LandenCode = "KY", LandenNaam = "Caymaneilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  125, LandenCode = "KZ", LandenNaam = "Kazachstan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  126, LandenCode = "LA", LandenNaam = "Laos", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  127, LandenCode = "LB", LandenNaam = "Libanon", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  128, LandenCode = "LC", LandenNaam = "Saint Lucia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  129, LandenCode = "LI", LandenNaam = "Liechtenstein", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  130, LandenCode = "LK", LandenNaam = "Sri Lanka", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  131, LandenCode = "LR", LandenNaam = "Liberia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  132, LandenCode = "LS", LandenNaam = "Lesotho", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  133, LandenCode = "LT", LandenNaam = "Litouwen", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  134, LandenCode = "LU", LandenNaam = "Luxemburg", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  135, LandenCode = "LV", LandenNaam = "Letland", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  136, LandenCode = "LY", LandenNaam = "Libië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  137, LandenCode = "MA", LandenNaam = "Marokko", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  138, LandenCode = "MC", LandenNaam = "Monaco", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  139, LandenCode = "MD", LandenNaam = "Moldavië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  140, LandenCode = "ME", LandenNaam = "Montenegro", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  141, LandenCode = "MF", LandenNaam = "Sint Maarten (Frans deel)", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  142, LandenCode = "MG", LandenNaam = "Madagaskar", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  143, LandenCode = "MH", LandenNaam = "Marshalleilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  144, LandenCode = "MK", LandenNaam = "Noord-Macedonië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  145, LandenCode = "ML", LandenNaam = "Mali", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  146, LandenCode = "MM", LandenNaam = "Myanmar", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  147, LandenCode = "MN", LandenNaam = "Mongolië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  148, LandenCode = "MO", LandenNaam = "Macau", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  149, LandenCode = "MP", LandenNaam = "Noordelijke Marianen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  150, LandenCode = "MQ", LandenNaam = "Martinique", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  151, LandenCode = "MR", LandenNaam = "Mauritanië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  152, LandenCode = "MS", LandenNaam = "Montserrat", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  153, LandenCode = "MT", LandenNaam = "Malta", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  154, LandenCode = "MU", LandenNaam = "Mauritius", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  155, LandenCode = "MV", LandenNaam = "Malediven", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  156, LandenCode = "MW", LandenNaam = "Malawi", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  157, LandenCode = "MX", LandenNaam = "Mexico", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  158, LandenCode = "MY", LandenNaam = "Maleisië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  159, LandenCode = "MZ", LandenNaam = "Mozambique", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  160, LandenCode = "NA", LandenNaam = "Namibië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  161, LandenCode = "NC", LandenNaam = "Nieuw-Caledonië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  162, LandenCode = "NE", LandenNaam = "Niger", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  163, LandenCode = "NF", LandenNaam = "Norfolk", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  164, LandenCode = "NG", LandenNaam = "Nigeria", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  165, LandenCode = "NI", LandenNaam = "Nicaragua", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  166, LandenCode = "NL", LandenNaam = "Nederland", WoonlandbeginselIndicatie = 0, Active = true },
                new Landen { Id =  167, LandenCode = "NO", LandenNaam = "Noorwegen", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  168, LandenCode = "NP", LandenNaam = "Nepal", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  169, LandenCode = "NR", LandenNaam = "Nauru", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  170, LandenCode = "NU", LandenNaam = "Niue", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  171, LandenCode = "NZ", LandenNaam = "Nieuw-Zeeland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  172, LandenCode = "OM", LandenNaam = "Oman", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  173, LandenCode = "PA", LandenNaam = "Panama", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  174, LandenCode = "PE", LandenNaam = "Peru", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  175, LandenCode = "PF", LandenNaam = "Frans-Polynesië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  176, LandenCode = "PG", LandenNaam = "Papoea-Nieuw-Guinea", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  177, LandenCode = "PH", LandenNaam = "Filipijnen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  178, LandenCode = "PK", LandenNaam = "Pakistan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  179, LandenCode = "PL", LandenNaam = "Polen", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  180, LandenCode = "PM", LandenNaam = "Saint-Pierre en Miquelon", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  181, LandenCode = "PN", LandenNaam = "Pitcairneilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  182, LandenCode = "PR", LandenNaam = "Puerto Rico", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  183, LandenCode = "PS", LandenNaam = "Palestina", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  184, LandenCode = "PT", LandenNaam = "Portugal", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  185, LandenCode = "PW", LandenNaam = "Palau", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  186, LandenCode = "PY", LandenNaam = "Paraguay", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  187, LandenCode = "QA", LandenNaam = "Qatar", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  188, LandenCode = "RE", LandenNaam = "Réunion", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  189, LandenCode = "RO", LandenNaam = "Roemenië", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  190, LandenCode = "RS", LandenNaam = "Servië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  191, LandenCode = "RU", LandenNaam = "Rusland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  192, LandenCode = "RW", LandenNaam = "Rwanda", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  193, LandenCode = "SA", LandenNaam = "Saoedi-Arabië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  194, LandenCode = "SB", LandenNaam = "Salomonseilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  195, LandenCode = "SC", LandenNaam = "Seychellen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  196, LandenCode = "SD", LandenNaam = "Sudan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  197, LandenCode = "SE", LandenNaam = "Zweden", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  198, LandenCode = "SG", LandenNaam = "Singapore", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  199, LandenCode = "SH", LandenNaam = "Sint-Helena, Ascension en Tristan da Cunha", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  200, LandenCode = "SI", LandenNaam = "Slovenië", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  201, LandenCode = "SJ", LandenNaam = "Spitsbergen en Jan Mayen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  202, LandenCode = "SK", LandenNaam = "Slowakije", WoonlandbeginselIndicatie = 1, Active = true },
                new Landen { Id =  203, LandenCode = "SL", LandenNaam = "Sierra Leone", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  204, LandenCode = "SM", LandenNaam = "San Marino", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  205, LandenCode = "SN", LandenNaam = "Senegal", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  206, LandenCode = "SO", LandenNaam = "Somalië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  207, LandenCode = "SR", LandenNaam = "Suriname", WoonlandbeginselIndicatie = 4, Active = true },
                new Landen { Id =  208, LandenCode = "SS", LandenNaam = "Zuid-Sudan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  209, LandenCode = "ST", LandenNaam = "Sao Tomé en Principe", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  210, LandenCode = "SV", LandenNaam = "El Salvador", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  211, LandenCode = "SX", LandenNaam = "Sint Maarten (Nederlands deel)", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  212, LandenCode = "SY", LandenNaam = "Syrië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  213, LandenCode = "SZ", LandenNaam = "Swaziland", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  214, LandenCode = "TC", LandenNaam = "Turks- en Caicoseilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  215, LandenCode = "TD", LandenNaam = "Tsjaad", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  216, LandenCode = "TF", LandenNaam = "Franse Zuidelijke en Antarctische Gebieden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  217, LandenCode = "TG", LandenNaam = "Togo", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  218, LandenCode = "TH", LandenNaam = "Thailand", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  219, LandenCode = "TJ", LandenNaam = "Tadzjikistan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  220, LandenCode = "TK", LandenNaam = "Tokelau", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  221, LandenCode = "TL", LandenNaam = "Oost-Timor", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  222, LandenCode = "TM", LandenNaam = "Turkmenistan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  223, LandenCode = "TN", LandenNaam = "Tunesië", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  224, LandenCode = "TO", LandenNaam = "Tonga", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  225, LandenCode = "TR", LandenNaam = "Turkije", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  226, LandenCode = "TT", LandenNaam = "Trinidad en Tobago", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  227, LandenCode = "TV", LandenNaam = "Tuvalu", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  228, LandenCode = "TW", LandenNaam = "Taiwan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  229, LandenCode = "TZ", LandenNaam = "Tanzania", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  230, LandenCode = "UA", LandenNaam = "Oekraïne", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  231, LandenCode = "UG", LandenNaam = "Uganda", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  232, LandenCode = "UM", LandenNaam = "Kleine afgelegen eilanden van de Verenigde Staten", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  233, LandenCode = "US", LandenNaam = "Verenigde Staten", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  234, LandenCode = "UY", LandenNaam = "Uruguay", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  235, LandenCode = "UZ", LandenNaam = "Oezbekistan", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  236, LandenCode = "VA", LandenNaam = "Vaticaanstad (de Heilige Stoel)", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  237, LandenCode = "VC", LandenNaam = "Saint Vincent en de Grenadines", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  238, LandenCode = "VE", LandenNaam = "Venezuela", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  239, LandenCode = "VG", LandenNaam = "Britse Maagdeneilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  240, LandenCode = "VI", LandenNaam = "Amerikaanse Maagdeneilanden", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  241, LandenCode = "VN", LandenNaam = "Vietnam", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  242, LandenCode = "VU", LandenNaam = "Vanuatu", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  243, LandenCode = "WF", LandenNaam = "Wallis en Futuna", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  244, LandenCode = "WS", LandenNaam = "Samoa", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  245, LandenCode = "YE", LandenNaam = "Jemen", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  246, LandenCode = "YT", LandenNaam = "Mayotte", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  247, LandenCode = "ZA", LandenNaam = "Zuid-Afrika", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  248, LandenCode = "ZM", LandenNaam = "Zambia", WoonlandbeginselIndicatie = 2, Active = true },
                new Landen { Id =  249, LandenCode = "ZW", LandenNaam = "Zimbabwe", WoonlandbeginselIndicatie = 2, Active = true }
            );
        });

        modelBuilder.Entity<Green>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Tabelloon).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.ZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.MetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.VerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderMetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderVerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterMetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterVerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.HasIndex(p => new
            {
                p.Id,
                p.Year,
                p.CountryId,
                p.TypeId,
            });
        });

        modelBuilder.Entity<White>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Tabelloon).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.ZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.MetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.VerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderMetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.EerderVerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterZonderLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterMetLoonheffingskorting).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.LaterVerrekendeArbeidskorting).HasColumnType("decimal(18, 5)");
            entity.HasIndex(p => new
            {
                p.Id,
                p.Year,
                p.CountryId,
                p.TypeId,
            });
        });

        modelBuilder.Entity<AOW>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasData(
                new AOW { Id = 1, GeborenNa = new DateTime(1947, 12, 31), GeborenVoor = new DateTime(1948, 12, 1), GerechtigdIn = 2013, ExtraMaandenNa65 = 1 },
                new AOW { Id = 2, GeborenNa = new DateTime(1948, 11, 30), GeborenVoor = new DateTime(1949, 11, 1), GerechtigdIn = 2014, ExtraMaandenNa65 = 2 },
                new AOW { Id = 3, GeborenNa = new DateTime(1949, 10, 31), GeborenVoor = new DateTime(1950, 10, 1), GerechtigdIn = 2015, ExtraMaandenNa65 = 3 },
                new AOW { Id = 4, GeborenNa = new DateTime(1950, 9, 30), GeborenVoor = new DateTime(1951, 7, 1), GerechtigdIn = 2016, ExtraMaandenNa65 = 6 },
                new AOW { Id = 5, GeborenNa = new DateTime(1951, 6, 30), GeborenVoor = new DateTime(1952, 4, 1), GerechtigdIn = 2017, ExtraMaandenNa65 = 9 },
                new AOW { Id = 6, GeborenNa = new DateTime(1952, 3, 31), GeborenVoor = new DateTime(1953, 1, 1), GerechtigdIn = 2018, ExtraMaandenNa65 = 12 },
                new AOW { Id = 7, GeborenNa = new DateTime(1952, 12, 31), GeborenVoor = new DateTime(1953, 9, 1), GerechtigdIn = 2019, ExtraMaandenNa65 = 16 },
                new AOW { Id = 8, GeborenNa = new DateTime(1953, 8, 31), GeborenVoor = new DateTime(1954, 9, 1), GerechtigdIn = 2020, ExtraMaandenNa65 = 16 },
                new AOW { Id = 9, GeborenNa = new DateTime(1954, 8, 31), GeborenVoor = new DateTime(1955, 9, 1), GerechtigdIn = 2021, ExtraMaandenNa65 = 16 },
                new AOW { Id = 10, GeborenNa = new DateTime(1955, 8, 31), GeborenVoor = new DateTime(1956, 6, 1), GerechtigdIn = 2022, ExtraMaandenNa65 = 19 },
                new AOW { Id = 11, GeborenNa = new DateTime(1956, 5, 31), GeborenVoor = new DateTime(1957, 3, 1), GerechtigdIn = 2023, ExtraMaandenNa65 = 22 },
                new AOW { Id = 12, GeborenNa = new DateTime(1957, 2, 28), GeborenVoor = new DateTime(1958, 1, 1), GerechtigdIn = 2024, ExtraMaandenNa65 = 24 },
                new AOW { Id = 13, GeborenNa = new DateTime(1958, 12, 31), GeborenVoor = new DateTime(1960, 1, 1), GerechtigdIn = 2026, ExtraMaandenNa65 = 24 },
                new AOW { Id = 14, GeborenNa = new DateTime(1957, 12, 31), GeborenVoor = new DateTime(1959, 1, 1), GerechtigdIn = 2025, ExtraMaandenNa65 = 24 }
            );

        });

        modelBuilder.Entity<PremiePercentagesSocialeVerzekeringen>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasData(
                new PremiePercentagesSocialeVerzekeringen
                {
                    Id = 1,
                    Jaar = 2022,
                    PremiePercentageAlgemeneOuderdomsWet = 17.90m,
                    PremiePercentageNabestaanden = 0.10m,
                    PremiePercentageWetLangdurigeZorg = 9.65m,
                    MaximaalSociaalverzekeringPremieloon = 59706.00m,
                    PremiePercentageAlgemeenWerkloosheidsFondsLaag = 2.7m,
                    PremiePercentageAlgemeenWerkloosheidsFondsHoog = 7.7m,
                    PremiePercentageUitvoeringsdfondsvoordeOverheid = 0.68m,
                    PremiePercentageWetArbeidsongeschikheidLaag = 5.49m,
                    PremiePercentageWetArbeidsongeschikheidHoog = 7.05m,
                    PremiePercentageWetKinderopvang = 0.5m,
                    PremiePercentageZiektekostenverzekeringWerkgeversbijdrage = 6.75m,
                    PremiePercentageZiektekostenverzekeringWerknemersbijdrage = 5.50m,
                    MaximaalZiektekostenverzekeringLoon = 59706.00m,
                    SocialeVerzekeringenRecordActief = true,
                    SocialeVerzekeringenRecordActiefVanaf = new DateTime(2022, 1, 1),
                    SocialeVerzekeringenRecordActiefTot = new DateTime(2022, 12, 31),
                });
        });

        modelBuilder.Entity<Berekeningen>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.InkomenWit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InkomenGroen).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InhoudingOpLoonWit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InhoudingOpLoonGroen).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AlgemeneHeffingskortingBedrag).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VerrekendeArbeIdskorting).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SociaalVerzekeringsloon).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragAlgemeenWerkloosheIdsFondsLaag).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragAlgemeenWerkloosheIdsFondsHoog).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragUitvoeringsFondsvoordeOverheId).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragWetArbeIdsOngeschikheIdLaag).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragWetArbeIdsOngeschikheIdHoog).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragWetKinderopvang).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragZiektekostenVerzekeringsWetLoon).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragWGAVastWerkgever).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragWGAVastWerknemer).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragFlexWerkgever).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragFlexWerknemer).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragZWFlex).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WerkgeverWHKPremieBedragTotaal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NettoTeBetalenSubTotaal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NettoTeBetalenEindTotaal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BasisDagen).HasColumnType("decimal(18, 2)");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ////
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_saveDatabaseHelper != null)
        {
            return await _saveDatabaseHelper.SaveChangesAsync(this, new[] { "auditlog" }, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
