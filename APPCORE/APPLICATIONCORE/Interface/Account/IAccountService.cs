using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Account
{
    public interface IAccountService
    {
        Task<List<AccountModel>> GetAccountByAccountIDAsync(int accountID);
        Task<List<Notification>> GetNotificationByAccountIDAsync(int accountID);
        Task<List<Notification>> GetNotificationsAsync();
        Task<List<NewpaperModel>> GetNewpaperAsync();

        Task Register(AccountModel account);

        Task<int> GetTotalUsersAsync();

        Task<List<AccountModel>> GetAccountByRoleIDAsync();
        Task<AccountModel> UpdateaccountAsync(int accountID, AccountModel account);

        Task AddAnswerAsync(int accountID, int productID, string descriptionAnswer, string fullnameAnswer, string emailAnswer);
        Task<IEnumerable<AnswerModel>> GetAnswersByProductIdAsync(int productId);
    }
}
