import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { FileUpload } from 'primeng/fileupload';
import { ServiceHelper } from '../../../helpers/service';

@Component({
  selector: 'psp-document',
  templateUrl: './belastingenUpload.component.html',
  providers: [MessageService]
})

export class BelastingenUploadComponent {
  constructor(
    private messageService: MessageService,
    private service: ServiceHelper
  ) { }

  uploadedFiles: any[] = [];
  errorFiles: any[] = [];
  progressVisible = false;
  showButton = true;

  removeFile(event: any, file: File, uploader: FileUpload) {
    const index = uploader.files.indexOf(file);
    uploader.remove(event,index);
  }

  uploadFile(event: any, formUpload: any) {
    this.progressVisible = true;
    var formData: FormData = new FormData();
    var hasError: boolean = false;

    this.showButton = false;

    for (let file of event.files) {
      formData.append('files', file, file.name);
    }
    
    this.service.post('document','documents/csv/upload', formData).subscribe((result) => {
      if (result) {
        hasError = result.insertToTaxResult.find((x: { isProcessed: boolean; }) => x.isProcessed == false);
          
        if (hasError) {
          this.setMessage('error', 'Error', result.message);
        } else {
          this.setMessage('success', 'Success', result.message);
        }
          
        this.progressVisible = false;
        formUpload.clear();
        this.uploadedFiles = result.insertToTaxResult;
      }
    });
  }

  onSelectUpload() {
    this.uploadedFiles = [];
    this.showButton = true;
  }

  setMessage(serverity: string, summary: string, message: string) {
    this.messageService.add({ key: 'tr', severity: serverity, summary: summary, detail: message, life: 10000 });
  }
}
