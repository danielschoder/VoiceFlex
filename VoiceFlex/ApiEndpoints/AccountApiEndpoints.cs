﻿using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class AccountApiEndpoints
{
    public static WebApplication MapAccountApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/accounts", CreateAccountAsync);
        app.MapGet("/api/accounts/{id:guid}/phonenumbers", GetAccountWithPhoneNumbersAsync);
        app.MapPatch("/api/accounts/{id:guid}", UpdateAccountAsync);

        return app;
    }

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "description": "John Mary Doe",
    ///         "status": 1
    ///     }
    /// </remarks>
    private static async Task<IResult> CreateAccountAsync(
        AccountDto account, IAccountManager accountManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await accountManager.CreateAccountAsync(account));

    /// <summary>
    /// Get all phone numbers for an account.
    /// </summary>
    /// <param name="id">Account id</param>
    private static async Task<IResult> GetAccountWithPhoneNumbersAsync(
        Guid id, IAccountManager accountManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await accountManager.GetAccountWithPhoneNumbersAsync(id));

    /// <summary>
    /// Set an account to active (1) or suspended (0).
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "status": 0
    ///     }
    /// </remarks>
    /// <param name="id">Account id</param>
    private static async Task<IResult> UpdateAccountAsync(
        Guid id, AccountUpdateDto accountUpdate, IAccountManager accountManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await accountManager.UpdateAccountAsync(id, accountUpdate));
}
