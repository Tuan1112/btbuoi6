using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using buoi6.Models;

namespace buoi6
{
    public partial class Form1 : Form
    {
        private StudentContextDB context;
        private Studentt selectedStudent;

        public Form1()
        {
            InitializeComponent();
        }

        private void FillFalcutyComboBox(List<Faculty> listFalcutys)
        {
            this.cboKhoa.DataSource = listFalcutys;
            this.cboKhoa.DisplayMember = "FacultyName";
            this.cboKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Studentt> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells[1].Value = item.FullName;
                dgvSinhVien.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvSinhVien.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void ClearForm()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            cboKhoa.SelectedIndex = -1;
            txtDiemTB.Clear();
        }

        private void LoadData()
        {
            try
            {
                context = new StudentContextDB();
                List<Faculty> listFalcutys = context.Faculties.ToList();
                List<Studentt> listStudents = context.Studentts.ToList();
                FillFalcutyComboBox(listFalcutys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                string studentID = row.Cells[0].Value.ToString();

                selectedStudent = context.Studentts.FirstOrDefault(s => s.StudentID == studentID);

                if (selectedStudent != null)
                {
                    txtMSSV.Text = selectedStudent.StudentID.ToString();
                    txtHoTen.Text = selectedStudent.FullName;
                    cboKhoa.SelectedValue = selectedStudent.FacultyID;
                    txtDiemTB.Text = selectedStudent.AverageScore.ToString();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Studentt newStudent = new Studentt()
                {
                    StudentID = txtMSSV.Text,
                    FullName = txtHoTen.Text,
                    FacultyID = Convert.ToInt32(cboKhoa.SelectedValue),
                    AverageScore = float.Parse(txtDiemTB.Text)
                };

                context.Studentts.Add(newStudent);
                context.SaveChanges();

                MessageBox.Show("Thêm sinh viên thành công.");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã số " + selectedStudent.StudentID + " ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Studentts.Remove(selectedStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công.");
                        ClearForm();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    selectedStudent.StudentID = txtMSSV.Text;
                    selectedStudent.FullName = txtHoTen.Text;
                    selectedStudent.FacultyID = Convert.ToInt32(cboKhoa.SelectedValue);
                    selectedStudent.AverageScore = float.Parse(txtDiemTB.Text);

                    context.SaveChanges();

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công.");
                    ClearForm();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
            }
        }
    }

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
