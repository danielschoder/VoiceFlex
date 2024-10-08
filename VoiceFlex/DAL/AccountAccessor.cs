﻿using Microsoft.EntityFrameworkCore;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.DAL;

public interface IAccountAccessor
{
    Task<Account> CreateAsync(AccountDto account);
    Task<AccountDto> GetAsync(Guid id);
    Task<Account> SetActiveAsync(Guid id);
    Task<Account> SetSuspendedAsync(Guid id);
}

public class AccountAccessor : IAccountAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public AccountAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Account> CreateAsync(AccountDto account)
    {
        var dbAccount = new Account(account);
        await _dbContext.VOICEFLEX_Accounts.AddAsync(dbAccount);
        await _dbContext.SaveChangesAsync();
        return dbAccount;
    }

    public async Task<AccountDto> GetAsync(Guid id)
        => await _dbContext.VOICEFLEX_Accounts
            .AsNoTracking()
            .Where(a => a.Id.Equals(id))
            .Include(a => a.PhoneNumbers)
            .Select(a => new AccountDto(a.Id, a.Description, a.Status, a.PhoneNumbers))
            .FirstOrDefaultAsync();

    public async Task<Account> SetActiveAsync(Guid id)
    {
        var dbAccount = await _dbContext.VOICEFLEX_Accounts.FindAsync(id);
        if (dbAccount is not null)
        {
            dbAccount.Status = AccountStatus.Active;
            await _dbContext.SaveChangesAsync();
        }
        return dbAccount;
    }

    public async Task<Account> SetSuspendedAsync(Guid id)
    {
        var dbAccount = await _dbContext.VOICEFLEX_Accounts
            .Where(a => a.Id.Equals(id))
            .Include(a => a.PhoneNumbers)
            .FirstOrDefaultAsync();
        if (dbAccount is not null)
        {
            foreach (var phoneNumber in dbAccount.PhoneNumbers)
            {
                phoneNumber.AccountId = null;
            }
            dbAccount.Status = AccountStatus.Suspended;
            await _dbContext.SaveChangesAsync();
        }
        return dbAccount;
    }
}
