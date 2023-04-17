import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { FileUpload } from 'primeng/fileupload';
import { ServiceHelper } from '../../../helpers/service';

@Component({
  selector: 'psp-employee-upload',
  templateUrl: './employeeUpload.component.html',
  providers: [MessageService]
})

export class EmployeeUploadComponent {
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
    
    this.service.post('document','documents/loonaangifte/upload/2022', formData).subscribe((result) => {
      if (result) {
        hasError = result.status.find((x: { isValid: boolean; }) => x.isValid == false);
          
        if (hasError) {
          this.setMessage('error', 'Fout', 'Ongeldige XML');
        } else {
          this.setMessage('success', 'Succes', 'Geldige XML');
        }

        this.progressVisible = false;
        formUpload.clear();
        this.uploadedFiles = result.status;
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
