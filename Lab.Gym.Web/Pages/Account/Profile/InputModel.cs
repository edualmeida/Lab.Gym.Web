using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using Lab.Gym.Web.Pages.Shared.Extensions;

namespace Lab.Gym.Web.Pages.Manage.Profile;

public class InputModel
{
    public string UserId { get; set; }
    
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Display(Name = "Role")]
    public string SelectedRole { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Maximum 300 characters")]
    [Display(Name = "Full name")]
    public string FullName { get; set; }

    [Required]
    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
    
    [DataType(DataType.Text)]
    [StringLength(50)]
    [Display(Name = "Address")]
    public string Address1 { get; set; }
    
    [DataType(DataType.Text)]
    [StringLength(50)]
    public string Address2 { get; set; }
    
    [DataType(DataType.Text)]
    [StringLength(25)]
    [Display(Name = "City")]
    public string City { get; set; }
    
    [DataType(DataType.Text)]
    [StringLength(25)]
    [Display(Name = "County")]
    public string County { get; set; }
    
    [DataType(DataType.Text)]
    [StringLength(10)]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }

    [DataType(DataType.Text)]
    [StringLength(50)]
    [Display(Name = "Phone 1")]
    public string Phone1 { get; set; }

    [DataType(DataType.Text)]
    [StringLength(50)]
    [Display(Name = "Phone 2")]
    public string Phone2 { get; set; }

    public static InputModel GetFromClaims(ClaimsIdentity claimsIdentity, string defaultValue = "-")
    {
        var claims = claimsIdentity.Claims;

        var Input = new InputModel();
        Input.FullName = claims.GetValue("FullName", defaultValue);
        Input.Address1 = claims.GetValue("Address1", defaultValue);
        Input.Address2 = claims.GetValue("Address2", defaultValue);
        Input.City = claims.GetValue("City", defaultValue);
        Input.County = claims.GetValue("County", defaultValue);
        Input.PostalCode = claims.GetValue("PostalCode", defaultValue);
        Input.Phone1 = claims.GetValue("Phone1", defaultValue);
        Input.Email = claims.GetValue("email", defaultValue);
        Input.UserId = claims.GetValue("sub", defaultValue);

        var birthDateString = claims.GetValue("BirthDate", "");
        if (!string.IsNullOrWhiteSpace(birthDateString))
        {
            Input.BirthDate = DateTime.ParseExact(birthDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        return Input;
    }
}