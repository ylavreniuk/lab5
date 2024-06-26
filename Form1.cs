using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLib;
using Lab3;

namespace Lab05
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Resize(object sender, EventArgs e)
        {
            int buttonsSize = 9 * btnAdd.Width + 3 * tsSeparator1.Width + 30;
            btnExit.Margin = new Padding(Width - buttonsSize, 0, 0, 0);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            gvCamera.AutoGenerateColumns = false;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Name";
            column.Name = "Назва";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Model";
            column.Name = "Модель";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Contry";
            column.Name = "Страна виробництва";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "SceenDiagonal";
            column.Name = "Діагональ екрану";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "MatrixSize";
            column.Name = "Розмір матриці";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "YearOfProduction";
            column.Name = "Рік виробництва";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Weight";
            column.Name = "Вага";
            gvCamera.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "LensInterchangeable";
            column.Name = "Має об'єктив";
            gvCamera.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Price";
            column.Name = "Ціна";
            gvCamera.Columns.Add(column);

            bindSrcCamera.Add(new Camera("Nikon", "D3500", "Japan", 3, 24, 2019, 365, true));
            EventArgs eventArgs = new EventArgs(); OnResize(eventArgs);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Camera camera = new Camera();

            fCamera fbc = new fCamera(camera);
            if (fbc.ShowDialog() == DialogResult.OK)
            {
                bindSrcCamera.Add(camera);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Camera camera = (Camera)bindSrcCamera.List[bindSrcCamera.Position];

            fCamera fbc = new fCamera(camera);
            if (fbc.ShowDialog() == DialogResult.OK)
            {
                bindSrcCamera.List[bindSrcCamera.Position] = camera;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Видалити поточний запис?", "Видалення запису", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            { bindSrcCamera.RemoveCurrent(); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити таблицю?\n\nВсі дані будуть втрачені", "Очищення даних", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                bindSrcCamera.List.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Закрити застосунок?", "Вихід з програми", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnSaveAsText_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у текстовому форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            StreamWriter sw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);
                try
                {
                    foreach (Camera bc in bindSrcCamera.List)
                    {
                        sw.Write(bc.Name + "\t" + bc.Model + "\t" +
                                 bc.Contry + "\t" + bc.SceenDiagonal + "\t" + bc.MatrixSize + "\t" +
                                 bc.YearOfProduction + "\t" + bc.Weight + "\t" + bc.LensInterchangeable + "\t" + bc.Price + "\t\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private void btnSaveAsBinary_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Файли даних (*.camera)|*.camera|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у бінарному форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            BinaryWriter bw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bw = new BinaryWriter(saveFileDialog.OpenFile());
                try
                {
                    foreach (Camera bc in bindSrcCamera.List)
                    {
                        bw.Write(bc.Name);
                        bw.Write(bc.Model);
                        bw.Write(bc.Contry);
                        bw.Write(bc.SceenDiagonal);
                        bw.Write(bc.MatrixSize);
                        bw.Write(bc.YearOfProduction);
                        bw.Write(bc.Weight);
                        bw.Write(bc.LensInterchangeable);
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    bw.Close();
                }
            }
        }

        private void btnOpenFromText_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.txt)| *.txt|All files (*.*) | *.* ";
            openFileDialog.Title = "Прочитати дані у текстовому форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            StreamReader sr;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcCamera.Clear(); sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
                string s;
                try
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] split = s.Split('\t');
                        Camera bc = new Camera(split[0], split[1], split[2], int.Parse(split[3]), int.Parse(split[4])
                            , double.Parse(split[5]), double.Parse(split[6]), bool.Parse(split[7]));
                        bindSrcCamera.Add(bc);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        private void btnOpenFromBinary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.camera)| *.camera|All files (*.*) | *.* ";
            openFileDialog.Title = "Прочитати дані у текстовому форматі";
            openFileDialog.InitialDirectory = Application.StartupPath;
            BinaryReader br;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcCamera.Clear();
                br = new BinaryReader(openFileDialog.OpenFile());
                try
                {
                    Camera camera;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        camera = new Camera();
                        for (int i = 0; i <= 8; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    camera.Name = br.ReadString();
                                    break;
                                case 2:
                                    camera.Model = br.ReadString();
                                    break;
                                case 3:
                                    camera.Contry = br.ReadString();
                                    break;
                                case 4:
                                    camera.SceenDiagonal = br.ReadInt32();
                                    break;
                                case 5:
                                    camera.MatrixSize = br.ReadInt32();
                                    break;
                                case 6:
                                    camera.YearOfProduction = br.ReadDouble();
                                    break;
                                case 7:
                                    camera.Weight = br.ReadDouble();
                                    break;
                                case 8:
                                    camera.LensInterchangeable = br.ReadBoolean();
                                    break;
                            }

                        }
                        bindSrcCamera.Add(camera);
                        
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Сталась помилка: \n{0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    br.Close();
                }
            }
        }
    }
}
