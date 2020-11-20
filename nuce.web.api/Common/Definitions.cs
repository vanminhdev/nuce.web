using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Common
{
    public class Definitions
    {
        public static Dictionary<string, string> Roles = new Dictionary<string, string>()
        {
            { "Admin", "Quản trị" },
            { "P_KhaoThi", "Phòng khảo thí" },
            { "P_CTSV", "Phòng công tác sinh viên" },
            { "K_CNTT", "Khoa CNTT" }
        };
    }

    public enum BackupType
    {
        Backup = 1,
        Restore = 2
    }

    /// <summary>
    /// Loại đợt khảo sát
    /// </summary>
    public enum SurveyRoundType
    {
        /// <summary>
        /// Đánh giá chất lượng giảng dạy
        /// </summary>
        RatingTeachingQuality = 1
    }
}
