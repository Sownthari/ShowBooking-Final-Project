import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminApiService } from '../admin-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-movie',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-movie.component.html',
  styleUrl: './add-movie.component.css'
})
export class AddMovieComponent {
  movie = {
    movieName: '',
    genre: '',
    language: '',
    durationMinutes: 0,
    releaseDate: '',
    description: ''
  };

  selectedFile: File | null = null;

  constructor(private adminApiService: AdminApiService, private router: Router) {}

  
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onSubmit() {
    // Create a new FormData object to send form data
    const formData = new FormData();
    
    // Append each property of the movie object directly to the FormData
    formData.append('MovieName', this.movie.movieName);
    formData.append('Genre', this.movie.genre);
    formData.append('Language', this.movie.language);
    formData.append('DurationMinutes', this.movie.durationMinutes.toString()); // Convert to string since FormData requires string/Blob
    formData.append('ReleaseDate', this.movie.releaseDate); 
    formData.append('Description', this.movie.description || '');

    
    if (this.selectedFile) {
        formData.append('ImageUrl', this.selectedFile, this.selectedFile.name);
    } else {
        console.error('No image file selected.');
        return; 
    }

    this.adminApiService.addMovie(formData).subscribe(
        response => {
            alert('Movie added successfully');
            this.router.navigate(['/movies-list']);
            
        },
        error => {
            console.error('Error adding movie:', error);
        }
    );
}

}
