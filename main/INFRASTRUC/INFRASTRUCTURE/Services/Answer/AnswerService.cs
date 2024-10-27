using APPLICATIONCORE.Interface.Answer;
using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Services.Answer
{
    public class AnswerService : IAnswerService
    {

        private readonly MyDbContext _context;

        public AnswerService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AnswerModel>> GetAllAnswers()
        {
            return await _context.Answers.ToListAsync(); 
        }
        public async Task<AnswerModel> GetAnswerById(int AnswerID)
        {
            return await _context.Answers.FindAsync(AnswerID);
        }
        public async Task AddAnswer(AnswerModel answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        private void BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        //hàm cập nhật sản phẩm
        public async Task<AnswerModel> UpdateAnswerAsync(int AnswerID, AnswerModel answer)
        {
            // Tìm sản phẩm theo ID
            var existinganswer = await _context.Answers.FindAsync(AnswerID);
            if (existinganswer == null)
            {
                throw new KeyNotFoundException("Không tìm thấy nhà cung cấp");
            }
            // Cập nhật các thuộc tính khác
            existinganswer.emailAnswer = answer.emailAnswer;
            existinganswer.fullnameAnswer = answer.fullnameAnswer;
            existinganswer.DescriptionAnswer = answer.DescriptionAnswer;
            existinganswer.accountID = answer.accountID;
            existinganswer.productID = answer.productID;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existinganswer;
        }

        public async Task DeleteAnswer(int AnswerID)
        {
            var answer = await _context.Answers.FindAsync(AnswerID);
            if (answer == null) throw new KeyNotFoundException("Không tìm thấy nhà cung cấp để xóa");

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<AnswerModel>> SearchAnswers(string name)
        {
            return await _context.Answers
                .Where(p => p.DescriptionAnswer.ToString().Contains(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<AnswerModel>> FindById(int AnswerID)
        {
            return await _context.Answers
                .Where(p => p.AnswerID.ToString().Contains(AnswerID.ToString()))
                .ToListAsync();
        }
    }
}
