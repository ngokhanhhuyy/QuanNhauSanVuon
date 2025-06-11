using System.Text.Json;
using System.Text.RegularExpressions;

namespace QuanNhauSanVuon.Services.Exceptions;

public class PropertyErrorDetail
{
    public object[] PropertyPathElements { get; set; }
    public string ErrorMessage { get; set; }
}