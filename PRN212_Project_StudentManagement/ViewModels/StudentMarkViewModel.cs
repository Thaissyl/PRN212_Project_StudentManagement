using PRN212_Project_StudentManagement.DTOs;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class StudentMarkViewModel : ViewModelBase
    {
        private ObservableCollection<StudentMarkViewDTO> _studentMarks;
        private ObservableCollection<StudentMarkViewDTO> _allStudentMarks;
        private StudentMarkViewDTO _selectedStudentMark;
        private string _selectedSemester;
        private string _selectedSubject;
        private string _selectedClass;
        private ObservableCollection<string> _semesters;
        private ObservableCollection<string> _subjects;
        private ObservableCollection<string> _classes;
        private User _currentUser;
        
        // Search functionality
        private string _searchText;
        
        // Details panel properties
        private bool _isEditing;
        private bool _isAdding;
        private string _editStudentName;
        private string _editClassName;
        private string _editSubjectName;
        private string _editSemesterName;
        private double _editMark;
        private DateTime _editExamDate;
        private string _editTeacherName;
        
        // Student list for adding new marks
        private ObservableCollection<StudentDTO> _studentList;
        private StudentDTO _selectedStudentForAdd;
        private ObservableCollection<Class> _classList;
        private Class _selectedClassForAdd;
        private ObservableCollection<Subject> _subjectList;
        private Subject _selectedSubjectForAdd;
        private ObservableCollection<Semester> _semesterList;
        private Semester _selectedSemesterForAdd;

        public ObservableCollection<StudentMarkViewDTO> StudentMarks
        {
            get => _studentMarks;
            set
            {
                _studentMarks = value;
                OnPropertyChanged(nameof(StudentMarks));
            }
        }

        public StudentMarkViewDTO SelectedStudentMark
        {
            get => _selectedStudentMark;
            set
            {
                _selectedStudentMark = value;
                OnPropertyChanged(nameof(SelectedStudentMark));
                OnPropertyChanged(nameof(IsViewing));
                if (value != null)
                {
                    LoadMarkDetails(value);
                }
                else
                {
                    ClearEditFields();
                }
            }
        }

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                FilterStudentMarks();
            }
        }

        public string SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                FilterStudentMarks();
            }
        }

        public string SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                FilterStudentMarks();
            }
        }

        public ObservableCollection<string> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
                OnPropertyChanged(nameof(Semesters));
            }
        }

        public ObservableCollection<string> Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                OnPropertyChanged(nameof(Subjects));
            }
        }

        public ObservableCollection<string> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        // Search property
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterStudentMarks();
            }
        }

        // Details panel properties
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(IsViewing));
            }
        }

        public bool IsAdding
        {
            get => _isAdding;
            set
            {
                _isAdding = value;
                OnPropertyChanged(nameof(IsAdding));
                OnPropertyChanged(nameof(IsViewing));
                if (value)
                {
                    LoadListsForAdd();
                }
            }
        }

        public string EditStudentName
        {
            get => _editStudentName;
            set
            {
                _editStudentName = value;
                OnPropertyChanged(nameof(EditStudentName));
            }
        }

        public string EditClassName
        {
            get => _editClassName;
            set
            {
                _editClassName = value;
                OnPropertyChanged(nameof(EditClassName));
            }
        }

        public string EditSubjectName
        {
            get => _editSubjectName;
            set
            {
                _editSubjectName = value;
                OnPropertyChanged(nameof(EditSubjectName));
            }
        }

        public string EditSemesterName
        {
            get => _editSemesterName;
            set
            {
                _editSemesterName = value;
                OnPropertyChanged(nameof(EditSemesterName));
            }
        }

        public double EditMark
        {
            get => _editMark;
            set
            {
                _editMark = value;
                OnPropertyChanged(nameof(EditMark));
            }
        }

        public DateTime EditExamDate
        {
            get => _editExamDate;
            set
            {
                _editExamDate = value;
                OnPropertyChanged(nameof(EditExamDate));
            }
        }

        public string EditTeacherName
        {
            get => _editTeacherName;
            set
            {
                _editTeacherName = value;
                OnPropertyChanged(nameof(EditTeacherName));
            }
        }

        // Lists for adding new marks
        public ObservableCollection<StudentDTO> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
            }
        }

        public StudentDTO SelectedStudentForAdd
        {
            get => _selectedStudentForAdd;
            set
            {
                _selectedStudentForAdd = value;
                OnPropertyChanged(nameof(SelectedStudentForAdd));
            }
        }

        public ObservableCollection<Class> ClassList
        {
            get => _classList;
            set
            {
                _classList = value;
                OnPropertyChanged(nameof(ClassList));
            }
        }

        public Class SelectedClassForAdd
        {
            get => _selectedClassForAdd;
            set
            {
                _selectedClassForAdd = value;
                OnPropertyChanged(nameof(SelectedClassForAdd));
            }
        }

        public ObservableCollection<Subject> SubjectList
        {
            get => _subjectList;
            set
            {
                _subjectList = value;
                OnPropertyChanged(nameof(SubjectList));
            }
        }

        public Subject SelectedSubjectForAdd
        {
            get => _selectedSubjectForAdd;
            set
            {
                _selectedSubjectForAdd = value;
                OnPropertyChanged(nameof(SelectedSubjectForAdd));
            }
        }

        public ObservableCollection<Semester> SemesterList
        {
            get => _semesterList;
            set
            {
                _semesterList = value;
                OnPropertyChanged(nameof(SemesterList));
            }
        }

        public Semester SelectedSemesterForAdd
        {
            get => _selectedSemesterForAdd;
            set
            {
                _selectedSemesterForAdd = value;
                OnPropertyChanged(nameof(SelectedSemesterForAdd));
            }
        }

        public ICommand BackCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand AddNewMarkCommand { get; }

        public bool IsViewing => SelectedStudentMark != null && !IsEditing && !IsAdding;

        public string TeacherFullName => _currentUser?.FullName;

        public StudentMarkViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _studentMarks = new ObservableCollection<StudentMarkViewDTO>();
            _allStudentMarks = new ObservableCollection<StudentMarkViewDTO>();
            _semesters = new ObservableCollection<string>();
            _subjects = new ObservableCollection<string>();
            _classes = new ObservableCollection<string>();
            _studentList = new ObservableCollection<StudentDTO>();
            _classList = new ObservableCollection<Class>();
            _subjectList = new ObservableCollection<Subject>();
            _semesterList = new ObservableCollection<Semester>();

            BackCommand = new ViewModelCommand(ExecuteBackCommand);
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            EditCommand = new ViewModelCommand(ExecuteEditCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand);
            AddCommand = new ViewModelCommand(ExecuteAddCommand);
            AddNewMarkCommand = new ViewModelCommand(ExecuteAddNewMarkCommand);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new DBContext())
                {
                    // Load student marks with related data (excluding semester with ID = 3)
                    var studentMarks = context.Marks
                        .Where(m => m.SemesterId != 3)
                        .Select(m => new StudentMarkViewDTO
                        {
                            MarkId = m.MarkId,
                            StudentId = m.StudentId,
                            StudentName = m.Student.User.FullName,
                            Email = m.Student.User.Email,
                            ClassName = m.Class.ClassName,
                            SubjectId = m.SubjectId,
                            SubjectName = m.Subject.SubjectName,
                            SemesterId = m.SemesterId,
                            SemesterName = m.Semester.SemesterName,
                            Mark = m.Mark1,
                            ExamDate = m.ExamDate.ToDateTime(TimeOnly.MinValue),
                            TeacherName = m.Class.Teacher != null ? m.Class.Teacher.User.FullName : "No Teacher Assigned"
                        })
                        .ToList();

                    _allStudentMarks = new ObservableCollection<StudentMarkViewDTO>(studentMarks);
                    StudentMarks = new ObservableCollection<StudentMarkViewDTO>(_allStudentMarks);

                    // Load semesters (excluding semester with ID = 3)
                    var semesters = context.Semesters
                        .Where(s => s.SemesterId != 3)
                        .Select(s => s.SemesterName)
                        .Distinct()
                        .OrderBy(s => s)
                        .ToList();
                    semesters.Insert(0, "All Semesters");
                    Semesters = new ObservableCollection<string>(semesters);
                    SelectedSemester = "All Semesters";

                    // Load subjects
                    var subjects = context.Subjects
                        .Select(s => s.SubjectName)
                        .Distinct()
                        .OrderBy(s => s)
                        .ToList();
                    subjects.Insert(0, "All Subjects");
                    Subjects = new ObservableCollection<string>(subjects);
                    SelectedSubject = "All Subjects";

                    // Load classes
                    var classes = context.Classes
                        .Select(c => c.ClassName)
                        .Distinct()
                        .OrderBy(c => c)
                        .ToList();
                    classes.Insert(0, "All Classes");
                    Classes = new ObservableCollection<string>(classes);
                    SelectedClass = "All Classes";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterStudentMarks()
        {
            try
            {
                var filteredMarks = _allStudentMarks.AsQueryable();

                if (!string.IsNullOrWhiteSpace(SelectedSemester) && SelectedSemester != "All Semesters")
                {
                    filteredMarks = filteredMarks.Where(sm => sm.SemesterName == SelectedSemester);
                }

                if (!string.IsNullOrWhiteSpace(SelectedSubject) && SelectedSubject != "All Subjects")
                {
                    filteredMarks = filteredMarks.Where(sm => sm.SubjectName == SelectedSubject);
                }

                if (!string.IsNullOrWhiteSpace(SelectedClass) && SelectedClass != "All Classes")
                {
                    filteredMarks = filteredMarks.Where(sm => sm.ClassName == SelectedClass);
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filteredMarks = filteredMarks.Where(sm => 
                        sm.StudentName.ToLower().Contains(SearchText.ToLower()));
                }

                StudentMarks = new ObservableCollection<StudentMarkViewDTO>(filteredMarks.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMarkDetails(StudentMarkViewDTO mark)
        {
            EditStudentName = mark.StudentName;
            EditClassName = mark.ClassName;
            EditSubjectName = mark.SubjectName;
            EditSemesterName = mark.SemesterName;
            EditMark = mark.Mark;
            EditExamDate = mark.ExamDate;
            EditTeacherName = mark.TeacherName;
        }

        private void ClearEditFields()
        {
            EditStudentName = string.Empty;
            EditClassName = string.Empty;
            EditSubjectName = string.Empty;
            EditSemesterName = string.Empty;
            EditMark = 0;
            EditExamDate = DateTime.Now;
            EditTeacherName = string.Empty;
        }

        private void LoadListsForAdd()
        {
            try
            {
                using (var context = new DBContext())
                {
                    // Load students
                    var students = context.Students
                        .Select(s => new StudentDTO
                        {
                            StudentID = s.StudentId,
                            FullName = s.User.FullName,
                            Email = s.User.Email
                        })
                        .OrderBy(s => s.FullName)
                        .ToList();
                    StudentList = new ObservableCollection<StudentDTO>(students);

                    // Load classes
                    var classes = context.Classes
                        .OrderBy(c => c.ClassName)
                        .ToList();
                    ClassList = new ObservableCollection<Class>(classes);

                    // Load subjects
                    var subjects = context.Subjects
                        .OrderBy(s => s.SubjectName)
                        .ToList();
                    SubjectList = new ObservableCollection<Subject>(subjects);

                    // Load semesters (excluding semester with ID = 3)
                    var semesters = context.Semesters
                        .Where(s => s.SemesterId != 3)
                        .OrderBy(s => s.SemesterName)
                        .ToList();
                    SemesterList = new ObservableCollection<Semester>(semesters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteBackCommand(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteRefreshCommand(object obj)
        {
            LoadData();
        }

        private void ExecuteEditCommand(object obj)
        {
            if (SelectedStudentMark != null)
            {
                IsEditing = true;
                IsAdding = false;
            }
        }

        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                using (var context = new DBContext())
                {
                    if (IsEditing && SelectedStudentMark != null)
                    {
                        // Update existing mark
                        var mark = context.Marks.Find(SelectedStudentMark.MarkId);
                        if (mark != null)
                        {
                            mark.Mark1 = EditMark;
                            mark.ExamDate = DateOnly.FromDateTime(EditExamDate);
                            context.SaveChanges();
                            
                            // Update the DTO
                            SelectedStudentMark.Mark = EditMark;
                            SelectedStudentMark.ExamDate = EditExamDate;
                            
                            MessageBox.Show("Mark updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else if (IsAdding)
                    {
                        // Check if mark already exists
                        var existingMark = context.Marks.FirstOrDefault(m => 
                            m.StudentId == SelectedStudentForAdd.StudentID &&
                            m.SubjectId == SelectedSubjectForAdd.SubjectId &&
                            m.SemesterId == SelectedSemesterForAdd.SemesterId);

                        if (existingMark != null)
                        {
                            MessageBox.Show("A mark for this student, subject, and semester already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Add new mark
                        var newMark = new Mark
                        {
                            StudentId = SelectedStudentForAdd.StudentID,
                            ClassId = SelectedClassForAdd.ClassId,
                            SubjectId = SelectedSubjectForAdd.SubjectId,
                            SemesterId = SelectedSemesterForAdd.SemesterId,
                            Mark1 = EditMark,
                            ExamDate = DateOnly.FromDateTime(EditExamDate)
                        };

                        context.Marks.Add(newMark);
                        context.SaveChanges();

                        MessageBox.Show("Mark added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    IsEditing = false;
                    IsAdding = false;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving mark: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            IsEditing = false;
            IsAdding = false;
            if (SelectedStudentMark != null)
            {
                LoadMarkDetails(SelectedStudentMark);
            }
        }

        private void ExecuteDeleteCommand(object obj)
        {
            if (SelectedStudentMark != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this mark?", "Confirm Delete", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var context = new DBContext())
                        {
                            var mark = context.Marks.Find(SelectedStudentMark.MarkId);
                            if (mark != null)
                            {
                                context.Marks.Remove(mark);
                                context.SaveChanges();
                                
                                MessageBox.Show("Mark deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadData();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting mark: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ExecuteAddCommand(object obj)
        {
            IsAdding = true;
            IsEditing = false;
            
            // Clear form
            EditStudentName = "";
            EditClassName = "";
            EditSubjectName = "";
            EditSemesterName = "";
            EditMark = 0;
            EditExamDate = DateTime.Now;
            EditTeacherName = "";
            
            SelectedStudentForAdd = null;
            SelectedClassForAdd = null;
            SelectedSubjectForAdd = null;
            SelectedSemesterForAdd = null;
        }

        private void ExecuteAddNewMarkCommand(object obj)
        {
            ExecuteSaveCommand(obj);
        }
    }
}
