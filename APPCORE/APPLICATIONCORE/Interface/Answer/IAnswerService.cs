using APPLICATIONCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Interface.Answer
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerModel>> GetAllAnswers();
        Task<AnswerModel> GetAnswerById(int AnswerID);
        Task AddAnswer(AnswerModel answer);
        Task<AnswerModel> UpdateAnswerAsync(int AnswerID, AnswerModel answer);
        Task DeleteAnswer(int AnswerID);
        Task<IEnumerable<AnswerModel>> SearchAnswers(string name);

        Task<IEnumerable<AnswerModel>> FindById(int AnswerID);
    }
}
