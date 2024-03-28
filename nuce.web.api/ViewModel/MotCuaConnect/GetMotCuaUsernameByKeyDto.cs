using System.ComponentModel.DataAnnotations;

namespace nuce.web.api.ViewModel.MotCuaConnect
{
    public class GetMotCuaUsernameByKeyDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống key")]
        public string Key { get; set; }
    }
}
