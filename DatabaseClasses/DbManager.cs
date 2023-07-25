using GestionScolaire.Models;
using Microsoft.Data.Sqlite;
using System.Reflection.PortableExecutable;

namespace GestionScolaire.DatabaseClasses
{
    public class DbManager
    {
        private readonly string connectionString;

        public DbManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Schools

        /**
        * List all schools
        * 
        * @return <see cref="List{School}"/>
        * */
        public List<School> GetSchools()
        {
            List<School> schools = new List<School>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM School", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schools.Add(CreateASchool(reader));
                    }
                }
            }

            return schools;
        }

        /**
         * Create new school, read school
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="School"/>
         * */
        private School CreateASchool(SqliteDataReader reader)
        {
            return new School
            {
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                SchoolName = Convert.ToString(reader["SchoolName"]),
                SchoolAdress = Convert.ToString(reader["SchoolAdress"]),
                SchoolCity = Convert.ToString(reader["SchoolCity"]),
                SchoolZipCode = Convert.ToString(reader["SchoolZipCode"]),
                SchoolCountry = Convert.ToString(reader["SchoolCountry"])
            };
        }

        /**
        * Add new school
        * 
        * @param <paramref name="School"/>
        * @return <see cref="int"/>
        * */
        public int AddSchool(School School)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO School (SchoolName, SchoolAdress, SchoolCity, SchoolZipCode, SchoolCountry) "
                                                        + "VALUES (@SchoolName, @SchoolAdress, @SchoolCity, @SchoolZipCode, @SchoolCountry); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@SchoolName", School.SchoolName);
                    command.Parameters.AddWithValue("@SchoolAdress", School.SchoolAdress);
                    command.Parameters.AddWithValue("@SchoolCity", School.SchoolCity);
                    command.Parameters.AddWithValue("@SchoolZipCode", School.SchoolZipCode);
                    command.Parameters.AddWithValue("@SchoolCountry", School.SchoolCountry);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a school by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="School|null"/>
        * */
        public School? GetSchoolById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM School WHERE SchoolId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateASchool(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a school
       * 
       * @param <paramref name="id"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateSchool(School school)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE School SET SchoolName=@schoolName, SchoolAdress=@schoolAdress, SchoolCity=@schoolCity, SchoolZipCode=@schoolZipCode, SchoolCountry=@schoolCountry WHERE SchoolId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", school.SchoolId);
                    command.Parameters.AddWithValue("@schoolName", school.SchoolName);
                    command.Parameters.AddWithValue("@schoolAdress", school.SchoolAdress);
                    command.Parameters.AddWithValue("@schoolCity", school.SchoolCity);
                    command.Parameters.AddWithValue("@schoolZipCode", school.SchoolZipCode);
                    command.Parameters.AddWithValue("@schoolCountry", school.SchoolCountry);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
      * Delete a school
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteSchool(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM School WHERE SchoolId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        #endregion
        
        #region Class

        /**
        * List all Classes
        * 
        * @return <see cref="List{Class}"/>
        * */
        public List<Classe> GetClasses()
        {
            List<Classe> classes = new List<Classe>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Classe", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(CreateAClass(reader));
                    }
                }
            }

            return classes;
        }

        /**
         * Create new school, read school
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="School"/>
         * */
        private Classe CreateAClass(SqliteDataReader reader)
        {
            return new Classe
            {
                ClassId = Convert.ToInt32(reader["ClassId"]),
                ClassName = Convert.ToString(reader["ClassName"]),
                SchoolId = Convert.ToInt32(reader["SchoolId"]),
                School = GetSchoolById(Convert.ToInt32(reader["SchoolId"]))
            };
        }

        /**
        * Add new Class
        * 
        * @param <paramref name="classe"/>
        * @return <see cref="int"/>
        * */
        public int AddClass(Classe classe)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO Classe (ClassName, SchoolId) "
                                                        + "VALUES (@ClassName, @SchoolId); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@ClassName", classe.ClassName);
                    command.Parameters.AddWithValue("@SchoolId", classe.SchoolId);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a school by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="Classe|null"/>
        * */
        public Classe? GetClassById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Classe WHERE ClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateAClass(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a Class
       * 
       * @param <paramref name="lass"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateClass(Classe lass)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Classe SET ClassName=@ClassName, SchoolId=@SchoolId WHERE ClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", lass.ClassId);
                    command.Parameters.AddWithValue("@ClassName", lass.ClassName);
                    command.Parameters.AddWithValue("@SchoolId", lass.SchoolId);
                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
      * Delete a Class
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteClass(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM Classe WHERE ClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
       * List all schools classes 
       * 
       * @return <see cref="List{Class}"/>
       * */
        public List<Classe> GetSchoolClasses(int schoolId)
        {
            List<Classe> classes = new List<Classe>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Classe WHERE SchoolId=@schoolId", conn))
                {
                    command.Parameters.AddWithValue("@schoolId", schoolId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(CreateAClass(reader));
                        }
                    }
                   
                }
                

            }

            return classes;
        }

        #endregion

        #region Subject

        /**
        * List all schools
        * 
        * @return <see cref="List{Subject}"/>
        * */
        public List<Subject> GetSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Subject", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(CreateASubject(reader));
                    }
                }
            }

            return subjects;
        }

        /**
         * Create new school, read school
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="School"/>
         * */
        private Subject CreateASubject(SqliteDataReader reader)
        {
            return new Subject
            {
                SubjectId = Convert.ToInt32(reader["SubjectId"]),
                SubjectName = Convert.ToString(reader["SubjectName"]),
                MaxPoint = Convert.ToInt32(reader["MaxPoint"]),
                ClassId = Convert.ToInt32(reader["ClassId"]),
                Class = GetClassById(Convert.ToInt32(reader["ClassId"]))
            };
        }

        /**
        * Add new subject
        * 
        * @param <paramref name="subject"/>
        * @return <see cref="int"/>
        * */
        public int AddSubject(Subject subject)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO Subject (SubjectName, MaxPoint, ClassId) "
                                                        + "VALUES (@SubjectName, @MaxPoint, @ClassId); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                    command.Parameters.AddWithValue("@MaxPoint", subject.MaxPoint);
                    command.Parameters.AddWithValue("@ClassId", subject.ClassId);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a Subject by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="Subject|null"/>
        * */
        public Subject? GetSubjectById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Subject WHERE SubjectId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateASubject(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a Subject
       * 
       * @param <paramref name="id"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateSubject(Subject subject)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Subject SET SubjectName=@SubjectName, MaxPoint=@MaxPoint, ClassId=@ClassId WHERE SubjectId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", subject.SubjectId);
                    command.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                    command.Parameters.AddWithValue("@MaxPoint", subject.MaxPoint);
                    command.Parameters.AddWithValue("@ClassId", subject.ClassId);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
      * Delete a Subjet
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteSubject(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM Subject WHERE SubjectId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
       * List a Class subject
       * @param <paramref name="classId"/>
       * @return <see cref="List{Subject}"/>
       * */
        public List<Subject> GetClassSubjects(int classId)
        {
            List<Subject> subjects = new List<Subject>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Subject WHERE ClassId=@classId", conn))
                {
                    command.Parameters.AddWithValue("@classId", classId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(CreateASubject(reader));
                        }
                    }
                }
            }

            return subjects;
        }

        #endregion

        #region Student

        /**
        * List all Student
        * 
        * @return <see cref="List{Student}"/>
        * */
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Student", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(CreateAStudent(reader));
                    }
                }
            }

            return students;
        }

        /**
         * Create new Student, read Student
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="Student"/>
         * */
        private Student CreateAStudent(SqliteDataReader reader)
        {
            return new Student
            {
                StudentId = Convert.ToInt32(reader["StudentId"]),
                FirstName = Convert.ToString(reader["FirstName"]),
                LastName = Convert.ToString(reader["LastName"]),
                StudentAdress = Convert.ToString(reader["StudentAdress"]),
                StudentCity = Convert.ToString(reader["StudentCity"]),
                StudentZipCode = Convert.ToString(reader["StudentZipCode"]),
                StudentCountry = Convert.ToString(reader["StudentCountry"])
            };
        }

        /**
        * Add new Student
        * 
        * @param <paramref name="Student"/>
        * @return <see cref="int"/>
        * */
        public int AddStudent(Student student)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO Student (FirstName, LastName, StudentAdress, StudentCity, StudentZipCode, StudentCountry) "
                                                        + "VALUES (@FirstName, @LastName, @StudentAdress, @StudentCity, @StudentZipCode, @StudentCountry); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@StudentAdress", student.StudentAdress);
                    command.Parameters.AddWithValue("@StudentCity", student.StudentCity);
                    command.Parameters.AddWithValue("@StudentZipCode", student.StudentZipCode);
                    command.Parameters.AddWithValue("@StudentCountry", student.StudentCountry);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a Student by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="School|null"/>
        * */
        public Student? GetStudentById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Student WHERE StudentId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateAStudent(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a Student
       * 
       * @param <paramref name="id"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateStudent(Student student)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Student SET FirstName=@FirstName, LastName=@LastName, StudentAdress=@StudentAdress, StudentCity=@StudentCity, StudentZipCode=@StudentZipCode, StudentCountry=@StudentCountry WHERE StudentId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", student.StudentId);
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@StudentAdress", student.StudentAdress);
                    command.Parameters.AddWithValue("@StudentCity", student.StudentCity);
                    command.Parameters.AddWithValue("@StudentZipCode", student.StudentZipCode);
                    command.Parameters.AddWithValue("@StudentCountry", student.StudentCountry);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
      * Delete a Student
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteStudent(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM Student WHERE StudentId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        #endregion

        #region StudentClass

        /**
        * List all StudentClass
        * 
        * @return <see cref="List{StudentClass}"/>
        * */
        public List<StudentClass> GetStudentClass()
        {
            List<StudentClass> studentClasses = new List<StudentClass>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM StudentClass", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentClasses.Add(CreateAStudentClass(reader));
                    }
                }
            }

            return studentClasses;
        }

        /**
         * Create new school, read school
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="School"/>
         * */
        private StudentClass CreateAStudentClass(SqliteDataReader reader)
        {
            return new StudentClass
            {
                StudentClassId = Convert.ToInt32(reader["StudentClassId"]),
                ClassId = Convert.ToInt32(reader["ClassId"]),
                StudentId = Convert.ToInt32(reader["StudentId"]),
                Year = Convert.ToInt32(reader["Year"]),
                Class = GetClassById(Convert.ToInt32(reader["ClassId"])),
                Student = GetStudentById(Convert.ToInt32(reader["StudentId"]))
            };
        }

        /**
        * Add new StudentClass
        * 
        * @param <paramref name="StudentClass"/>
        * @return <see cref="int"/>
        * */
        public int AddStudentClass(StudentClass studentClass)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO StudentClass (ClassId, StudentId, Year) "
                                                        + "VALUES (@ClassId, @StudentId, @Year); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@ClassId", studentClass.ClassId);
                    command.Parameters.AddWithValue("@StudentId", studentClass.StudentId);
                    command.Parameters.AddWithValue("@Year", studentClass.Year);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a StudentClass by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="StudentClass|null"/>
        * */
        public StudentClass? GetStudentClassById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM StudentClass WHERE StudentClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateAStudentClass(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a StudentClass
       * 
       * @param <paramref name="id"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateStudentClass(StudentClass studentClass)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE StudentClass SET ClassId=@ClassId, StudentId=@StudentId, Year=@Year WHERE StudentClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", studentClass.StudentClassId);
                    command.Parameters.AddWithValue("@ClassId", studentClass.ClassId);
                    command.Parameters.AddWithValue("@StudentId", studentClass.StudentId);
                    command.Parameters.AddWithValue("@Year", studentClass.Year);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
      * Delete a StudentClass
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteStudentClass(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM StudentClass WHERE StudentClassId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        #endregion

        #region CourseNote

        /**
        * List all CourseNote
        * 
        * @return <see cref="List{StudentClass}"/>
        * */
        public List<CourseNote> GetCourseNote()
        {
            List<CourseNote> courseNotes = new List<CourseNote>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM CourseNote", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courseNotes.Add(CreateACourseNote(reader));
                    }
                }
            }

            return courseNotes;
        }

        /**
         * Create new CourseNote, read CourseNote
         * 
         * @param <paramref name="reader"/>
         * @return <see cref="CourseNote"/>
         * */
        private CourseNote CreateACourseNote(SqliteDataReader reader)
        {
            return new CourseNote
            {
                CourseNoteId = Convert.ToInt32(reader["CourseNoteId"]),
                Mark = Convert.ToInt32(reader["Mark"]),
                StudentId = Convert.ToInt32(reader["StudentId"]),
                SubjectId = Convert.ToInt32(reader["SubjectId"]),
                Subject = GetSubjectById(Convert.ToInt32(reader["SubjectId"])),
                Student = GetStudentById(Convert.ToInt32(reader["StudentId"]))
            };
        }

        /**
        * Add new CourseNote
        * 
        * @param <paramref name="StudentClass"/>
        * @return <see cref="int"/>
        * */
        public int AddCourseNote(CourseNote courseNote)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO CourseNote (Mark, SubjectId, StudentId) "
                                                        + "VALUES (@Mark, @SubjectId, @StudentId); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@Mark", courseNote.Mark);
                    command.Parameters.AddWithValue("@SubjectId", courseNote.SubjectId);
                    command.Parameters.AddWithValue("@StudentId", courseNote.StudentId);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new School ID.");
                    }
                }
            }
        }

        /**
        * Find a CourseNote by his ID
        * 
        * @param <paramref name="id"/>
        * @return <see cref="CourseNote|null"/>
        * */
        public CourseNote? GetCourseNoteById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM CourseNote WHERE CourseNoteId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateACourseNote(reader);
                        }
                    }
                }


            }
            return null;
        }

        /**
       * Update a CourseNote
       * 
       * @param <paramref name="id"/>
       * @return <see cref="bool"/>
       * */

        public bool UpdateCourseNote(CourseNote courseNote)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE CourseNote SET Mark=@Mark, SubjectId=@SubjectId, StudentId=@StudentId WHERE CourseNoteId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", courseNote.CourseNoteId);
                    command.Parameters.AddWithValue("@Mark", courseNote.Mark);
                    command.Parameters.AddWithValue("@SubjectId", courseNote.SubjectId);
                    command.Parameters.AddWithValue("@StudentId", courseNote.StudentId);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

      /**
      * Delete a CourseNote
      * 
      * @param <paramref name="id"/>
      * @return <see cref="bool"/>
      * */
        public bool DeleteCourseNote(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM CourseNote WHERE CourseNoteId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        /**
        * List a student note in class
        * 
        * @param <paramref name="subjectId"/>
        * @param <paramref name="studentId"/>
        * @return <see cref="List{CourseNote}"/>
        * */
        public List<CourseNote> GetStudentSubjectNote(int subjectId, int studentId)
        {
            List<CourseNote> courseNotes = new List<CourseNote>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM CourseNote WHERE SubjectId=@subjectId AND StudentId=@studentId", conn))
                {
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.Parameters.AddWithValue("@studentId", studentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courseNotes.Add(CreateACourseNote(reader));
                        }
                    }
                }
            }

            return courseNotes;
        }
        
     /**
     * List a student note in class
     * 
     * @param <paramref name="classId"/>
     * @param <paramref name="classId"/>
     * @return <see cref="List{Subject}"/>
     * */
        public List<CourseNote> GetStudentNotes(int classId, int studentId)
        {
            List<CourseNote> courseNotes = new List<CourseNote>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM CourseNote WHERE ClassId=@classId AND AND StudentId=@studentId", conn))
                {
                    command.Parameters.AddWithValue("@classId", classId);
                    command.Parameters.AddWithValue("@studentId", studentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courseNotes.Add(CreateACourseNote(reader));
                        }
                    }
                }
            }

            return courseNotes;
        }

        #endregion
    }
}
