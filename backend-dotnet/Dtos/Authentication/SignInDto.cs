using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Dtos;

public class SignInDto : IRequestDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public void TransformValues()
    {
    }
}