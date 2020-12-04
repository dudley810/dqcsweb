using System.ComponentModel.DataAnnotations;

namespace dqcsweb.Models
{
    public class ContactUsModel
    {
        [Display(Name = "Name:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Text, ErrorMessage = "Field can't be empty")]
        public string Name { get; set; }

        [Display(Name = "Phone:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Display(Name = "Subject:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Text, ErrorMessage = "Field can't be empty")]
        public string Subject { get; set; }

        [Display(Name = "Message:")]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Text, ErrorMessage = "Field can't be empty")]
        public string Message { get; set; }

        public string recaptcharesponse { get; set; }
    }
}