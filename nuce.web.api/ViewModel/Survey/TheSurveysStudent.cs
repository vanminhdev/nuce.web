using nuce.web.api.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class TheSurveysStudent
    {
        public Guid Id { get; set; }
        public Guid BaiKhaoSatId { get; set; }
        public string StudentCode { get; set; }
        public string LecturerCode { get; set; }
        public string LecturerName { get; set; }
        public string ClassRoomCode { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int SubjectType { get; set; }
        public string DepartmentCode { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }

    public class SelectedAnswerAutoSave
    {
        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        public string ClassRoomCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        public string QuestionCode { get; set; }

        [NotContainWhiteSpace]
        public string AnswerCode { get; set; }

        public string answerCodeInMulSelect { get; set; }

        public bool isAnswerCodesAdd { get; set; }

        public string AnswerContent { get; set; }
    }
}
