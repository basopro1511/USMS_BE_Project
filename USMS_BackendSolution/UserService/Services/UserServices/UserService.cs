using BusinessObject;
using BusinessObject.ModelDTOs;
using UserService.Repository.UserRepository;

namespace UserService.Services.UserServices
    {
    public class UserService
        {
        private readonly IUserRepository _userRepository;

        public UserService()
            {
            _userRepository=new UserRepository();
            }
        /// <summary>
        /// Get all user
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse> GetAllUser()
            {
            APIResponse aPIResponse = new APIResponse();
            List<UserDTO> users =await _userRepository.GetAllUser();
            if (users==null||users.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy người dùng";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=users;
                }
            return aPIResponse;
            }

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetUserById(string userId)
            {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = await _userRepository.GetUserById(userId);
            if (user==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy người dùng với mã: " + userId ;
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=user;
                }
            return aPIResponse;
            }

        }
    }
