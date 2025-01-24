using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository.StudentRepository
{
    public class StudentRepository: IStudentRepository
    {
        #region Get All Student
        /// <summary>
        /// show a list full of student
        /// </summary>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public List<StudentDTO> GetAllStudent()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<Student> students = dbContext.Student
                        .Include(s => s.User)
                        .ThenInclude(u => u.Role)
                        .Include(s => s.Major)
                        .ToList();
                    List<StudentDTO> studentDTOs = new List<StudentDTO>();
                    foreach (var student in students)
                    {
                        StudentDTO studentDTO = new StudentDTO();
                        studentDTO.CopyProperties(student);
                        studentDTO.CopyProperties(student.User);
                        studentDTO.RoleName = student.User?.Role?.RoleName;
                        studentDTO.MajorName = student.Major?.MajorName;
                        studentDTOs.Add(studentDTO);
                    }
                    return studentDTOs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching users: " + ex.Message);
            }
        }
        #endregion

        #region Get Student By ID
        /// <summary>
        /// admin use to retrive a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public StudentDTO GetStudentById(string id)
        {
            try
            {
                var students = GetAllStudent();
                StudentDTO studentDTO = students.FirstOrDefault(x => x.UserId == id);
                return studentDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Student
        /// <summary>
        /// admin add a new user
        /// </summary>
        /// <param name="addStudentDTO"></param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public bool AddNewStudent(StudentDTO StudentDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    // Create a new User
                    var user = new User();
                    user.CopyProperties(StudentDTO);
                    dbContext.User.Add(user);
                    dbContext.SaveChanges(); // Save the User first to get the UserId

                    // Create a new Student
                    var student = new Student
                    {
                        StudentId = user.UserId,
                        MajorId = StudentDTO.MajorId,
                        Term = StudentDTO.Term
                    };
                    dbContext.Student.Add(student);
                    dbContext.SaveChanges(); // Save the Student
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Student
        /// <summary>
        /// admin update a user
        /// </summary>
        /// <param name="updateStudentDTO"></param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateStudent(StudentDTO updateStudentDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingStudent = dbContext.Student.Include(s => s.User).FirstOrDefault(s => s.User.UserId == updateStudentDTO.UserId);
                    //existingStudent.CopyProperties(updateStudentDTO);
                    existingStudent.User.FirstName = updateStudentDTO.FirstName ?? existingStudent.User.FirstName;
                    existingStudent.User.MiddleName = updateStudentDTO.MiddleName ?? existingStudent.User.MiddleName;
                    existingStudent.User.LastName = updateStudentDTO.LastName ?? existingStudent.User.LastName;
                    existingStudent.User.PersonalEmail = updateStudentDTO.PersonalEmail ?? existingStudent.User.PersonalEmail;
                    existingStudent.User.PhoneNumber = updateStudentDTO.PhoneNumber ?? existingStudent.User.PhoneNumber;
                    existingStudent.User.UserAvartar = updateStudentDTO.UserAvartar ?? existingStudent.User.UserAvartar;
                    existingStudent.User.DateOfBirth = updateStudentDTO.DateOfBirth != default ? updateStudentDTO.DateOfBirth : existingStudent.User.DateOfBirth;
                    existingStudent.User.UpdatedAt = DateTime.Now;
                    existingStudent.User.Address = updateStudentDTO.Address;
                    existingStudent.User.Status = updateStudentDTO.Status != 0 ? updateStudentDTO.Status : existingStudent.User.Status;
                    //Nếu cần cập nhật các thuộc tính liên quan đến sinh viên
                    existingStudent.MajorId = updateStudentDTO.MajorId ?? existingStudent.MajorId;
                    existingStudent.Term = updateStudentDTO.Term != 0 ? updateStudentDTO.Term : existingStudent.Term;
                    dbContext.Entry(existingStudent).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Student Status
        /// <summary>
        /// Update status for student
        /// </summary>
        /// <param name="id"></param>
        /// <returnstrue if success></returns>
        /// <exception cref="Exception"></exception>
        public bool UpdateStudentStatus(string id, int status)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(u => u.UserId == id);
                    if (existingUser != null)
                    {
                        existingUser.Status = status;
                        dbContext.Entry(existingUser).State = EntityState.Modified;
                        dbContext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }

}
