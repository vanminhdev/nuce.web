using nuce.web.api.Models.Survey.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Base
{
    public class BaiKhaoSatSinhVienServiceBase
    {
        private List<string> AddOrRemoveAnswerCodes(List<string> list, string answerCodeInMulSelect, bool isAnswerCodesAdd)
        {
            if (isAnswerCodesAdd) //thêm
            {
                if (list != null && !list.Contains(answerCodeInMulSelect)) //chưa có đáp án chọn nhiều này
                {
                    list.Add(answerCodeInMulSelect.Trim());
                }
                else if (list == null) //chưa có bất kì đáp án chọn nhiều nào
                {
                    list = new List<string>() { answerCodeInMulSelect };
                }
            }
            else //bỏ
            {
                if (list != null && list.Contains(answerCodeInMulSelect)) //có đáp án chọn nhiều này
                {
                    list.Remove(answerCodeInMulSelect.Trim());
                }

                if (list.Count == 0) //nếu k còn phần tử nào thì bỏ hẳn
                    return null;
            }
            return list;
        }

        protected string AutoSaveBaiLam(List<SelectedAnswer> selectedAnswers, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, 
            int? numStar, string city, bool isAnswerCodesAdd = true)
        {
            var exsist = false; //đã tồn tại câu hỏi chưa
            //cập nhật cho câu hỏi tương ứng
            foreach (var item in selectedAnswers)
            {
                if (item.QuestionCode == questionCode)
                {
                    if (answerCode != null) //lựa chọn chọn 1
                    {
                        item.AnswerCode = answerCode.Trim();
                    }
                    else if (answerCodeInMulSelect != null) // lựa chọn chọn nhiều
                    {
                        item.AnswerCodes = AddOrRemoveAnswerCodes(item.AnswerCodes, answerCodeInMulSelect, isAnswerCodesAdd);
                    }
                    else if (numStar != null)
                    {
                        item.NumStart = numStar;
                    }
                    else if (city != null)
                    {
                        item.City = city;
                    }
                    item.AnswerContent = answerContent; //câu trả lời text
                    exsist = true;
                    break;
                }
            }
            //thêm mới cho câu hỏi tương ứng
            if (!exsist)
            {
                var newSelectedAnswer = new SelectedAnswer
                {
                    QuestionCode = questionCode,
                    AnswerCode = answerCode, //là câu chọn 1
                    AnswerContent = answerContent
                };

                if (questionCode.Split('_').Length == 2) //là câu hỏi con của đáp án
                {
                    newSelectedAnswer.IsAnswerChildQuestion = true;
                }
                
                if (answerCodeInMulSelect != null && isAnswerCodesAdd) // lựa chọn chọn nhiều
                {
                    newSelectedAnswer.AnswerCodes = new List<string>() { answerCodeInMulSelect };
                }
                else if (numStar != null)
                {
                    newSelectedAnswer.NumStart = numStar;
                }
                else if (city != null)
                {
                    newSelectedAnswer.City = city;
                }

                selectedAnswers.Add(newSelectedAnswer);
            }

            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
            return JsonSerializer.Serialize(selectedAnswers, options);
        }
    }
}
