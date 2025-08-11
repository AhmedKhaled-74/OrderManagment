using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs.UserDTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters and spaces.")]
        public string? FullName { get; set; }


        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Remote("CheckEmailExists", "Account", ErrorMessage = "Email already exists.")]
        public string? Email { get; set; }


        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[_@*-])[a-zA-Z0-9_@*-]{10,21}$",
            ErrorMessage = "Password must contain at least one character from each category (letters, numbers, and special characters _, @, -, *)")]
        public string Password { get; set; } = null!;


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }


        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Region can only contain letters and spaces.")]
        public string? Region { get; set; }


        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9\s]+$", ErrorMessage = "Phone number can only contain numbers and spaces, and may start with a +.")]
        public string? PhoneNumber { get; set; }


        [StringLength(350, ErrorMessage = "Profile picture URL cannot exceed 350 characters.")]
        [Url(ErrorMessage = "Please enter a valid URL for the profile picture.")]
        [RegularExpression(@"^(https?://|/)[^\s/$.?#].[^\s]*$", ErrorMessage = "Invalid URL format.")]
        public string? ProfilePictureUrl { get; set;} // URL to the user's profile picture

        public AppUser ToAppUser()
        {
            return new AppUser
            {
                UserName = Email,
                Email = Email,
                FullName = FullName??"",
                DateOfBirth = DateOfBirth,
                Region = Region,
                PhoneNumber = PhoneNumber,
                ProfilePictureUrl = ProfilePictureUrl
            };
        }

    }
}
