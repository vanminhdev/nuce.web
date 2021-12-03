using nuce.web.api.Models.Survey.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Base
{
    public class ThongKeServiceBase
    {
        /// <summary>
        /// Tính toán câu trả lời
        /// </summary>
        /// <param name="selectedList"></param>
        /// <returns></returns>
        protected List<AnswerSelectedReportTotal> AnswerSelectedReportTotal(List<SelectedAnswerExtend> selectedList)
        {
            var result = new List<AnswerSelectedReportTotal>();

            //loại câu chọn 1
            var groupSingleChoiceSelectedAnswers = selectedList.Where(o => o.AnswerCode != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.AnswerCode });
            foreach (var item in groupSingleChoiceSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    AnswerCode = item.Key.AnswerCode,
                    Total = item.Count()
                });
            }

            //loại câu chọn nhiều
            var groupMultiChoiceSelectedAnswers = selectedList
                .Where(o => o.AnswerCodes != null && o.AnswerCodes.Count > 0)
                .SelectMany(o => o.AnswerCodes, (r, AnswerCode) => new { r.TheSurveyId, r.QuestionCode, AnswerCode })
                .GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.AnswerCode });

            foreach (var item in groupMultiChoiceSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    AnswerCode = item.Key.AnswerCode,
                    Total = item.Count()
                });
            }

            //loại câu trả lời text
            var groupShortAnswerSelectedAnswers = selectedList.Where(o => o.AnswerContent != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode }, o => new { o.AnswerContent });
            foreach (var item in groupShortAnswerSelectedAnswers)
            {
                List<string> strAllAnswerContent = new List<string>();
                foreach (var str in item)
                {
                    strAllAnswerContent.Add(str.AnswerContent);
                }

                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = JsonSerializer.Serialize(strAllAnswerContent)
                });
            }

            //loại vote sao
            var groupVoteStarSelectedAnswers = selectedList.Where(o => o.NumStart != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.NumStart }, o => new { o.NumStart });
            foreach (var item in groupVoteStarSelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = $"{item.Key.NumStart} sao",
                    Total = item.Count()
                });
            }

            //loại tỉnh thành
            var groupCitySelectedAnswers = selectedList.Where(o => o.City != null).GroupBy(o => new { o.TheSurveyId, o.QuestionCode, o.City }, o => new { o.City });
            foreach (var item in groupCitySelectedAnswers)
            {
                result.Add(new AnswerSelectedReportTotal
                {
                    TheSurveyId = item.Key.TheSurveyId,
                    QuestionCode = item.Key.QuestionCode,
                    Content = item.Key.City,
                    Total = item.Count()
                });
            }

            return result;
        }
    }
}
