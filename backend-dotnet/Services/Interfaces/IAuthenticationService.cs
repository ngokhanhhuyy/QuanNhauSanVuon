using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Services.Exceptions;

namespace QuanNhauSanVuon.Services.Interfaces;

/// <summary>
/// A service to handle authentication-related operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Signs in with the specified username and password using cookies.
    /// </summary>
    /// <param name="dto">
    /// An instance of the <see cref="SignInDto"/> class, containing the username and
    /// the password for the sign in operation.
    /// </param>
    /// <returns>
    /// A <see cref="int"/> representing the id of the signed in user.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the user with the specified username doesn't exist or has already been deleted.
    /// - When the specified password is incorrect.
    /// </exception>
    Task<int> SignInAsync(SignInDto dto);

    /// <summary>
    /// Signs out and clear the cookies which contains the authentication credentials from the
    /// requesting user.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SignOutAsync();
}