using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using ISUZU_NEXT.Server.Core.Extentions;

namespace UserService.Repository.TeacherRepository
    {
    public class TeacherRepository : ITeacherRepository 
        {
        #region Get All Teacher
        /// <summary>
        /// Get All Teacher from Database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<UserDTO> GetAllTeacher()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = dbcontext.User.Where(x => x.RoleId==4).ToList();
                    List<UserDTO> userDTOs = new List<UserDTO>();
                    foreach (var item in user)
                        {
                        UserDTO userDTO = new UserDTO();
                        userDTO.CopyProperties(item);
                        userDTOs.Add(userDTO);
                        dbcontext.SaveChanges();
                        }
                    return userDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get All Teacher Available by Major Id ( Status = 1 )
        /// <summary>
        /// Get All Teacher Available by Major Id ( Status = 1 )
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<UserDTO> GetAllTeacherAvailableByMajorId(string majorId)
            {
            try
                {
                var users = GetAllTeacher();
                var teacherAvailable = users.Where(x => x.Status==1 && x.MajorId == majorId).ToList();
                return teacherAvailable;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        }
    }
