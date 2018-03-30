using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using t1.Commons;
using t1.Models;

namespace t1
{
    public partial class Form1 : Form
    {
        private List<DataModel> listData = new List<DataModel>();

        public Form1()
        {
            InitializeComponent();
        }

        private void importExcel_Click(object sender, EventArgs e)
        {
            #region 从Excel获得用户登陆数据
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //string initialDirectory = System.Environment.CurrentDirectory;
                    openFileDialog.InitialDirectory = @"C:\Data";
                    //openFileDialog.InitialDirectory = initialDirectory;
                    openFileDialog.Filter = "Excel File |*.xlsxx;*.xlsx";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string excelFilePath = openFileDialog.FileName;
                        txtExcelPath.Text = excelFilePath;
                        DataTable dt = ExcelHelper.ExecuteQuery(excelFilePath, "select * from `Sheet1$`");
                        //StringBuilder sBuilder = new StringBuilder();
                        listData.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            DataModel model = new DataModel();
                            model.Code = row[0].ToString().Trim();
                            model.OldCode = row[1].ToString().Trim();
                            model.Name = row[2].ToString().Trim();
                            listData.Add(model);
                            //sBuilder.Clear();
                            //sBuilder.Append(row[0].ToString().Trim() + "\t");
                            //sBuilder.Append(row[1].ToString().Trim() + "\t");
                            //sBuilder.Append(row[2].ToString().Trim() + "\t");
                            //sBuilder.Append(row[3].ToString().Trim() + "\t");
                            //sBuilder.Append(row[4].ToString().Trim() + "\t");
                            //string str = sBuilder.ToString();
                            //if (!string.IsNullOrEmpty(str))
                            //{
                            //    listData.Add(str);
                            //}
                        }

                        txtMsg.Text = "导入Excel数据成功!\r\n";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            #endregion
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;

            foreach (var sourceItem in listData)
            {
                try
                {
                    using (var ctx = new SCVEntities())
                    {
                        //string oldDate = string.IsNullOrWhiteSpace(sourceItem.OldDate) ? null : sourceItem.OldDate;

                        ITEM matchedItem = ctx.Set<ITEM>().FirstOrDefault(i => i.COMPANY == "RB" && i.ITEM1 == sourceItem.OldCode && i.PACKING_CLASS == null);

                        if (matchedItem != null)
                        {
                            matchedItem.PACKING_CLASS = "Matched";
                            matchedItem.ITEM1 = sourceItem.Code;
                            matchedItem.ITEM_DESC = sourceItem.Name;

                            var matchedItemUnit = ctx.Set<ITEM_UNIT_OF_MEASURE>().FirstOrDefault(u => u.ITEM == sourceItem.OldCode && u.COMPANY == "RB" && u.ITEM_CLASS == null);
                            if (matchedItemUnit != null)
                            {
                                matchedItemUnit.ITEM = sourceItem.Code;
                                matchedItemUnit.ITEM_CLASS = "Matched";
                            }

                            var locationInvs = ctx.Set<LOCATION_INVENTORY>().Where(l => l.USER_DEF8 == null && l.ITEM == sourceItem.OldCode && l.COMPANY == "RB");
                            foreach (var li in locationInvs)
                            {
                                li.ITEM = sourceItem.Code;
                                li.ITEM_DESC = sourceItem.Name;
                                li.USER_DEF8 = "Matched";
                            }
                        }

                        ctx.SaveChanges();
                        txtMsg.AppendText(sourceItem.Code + ":" + sourceItem.Name + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    sourceItem.Msg = ex.Message;
                    txtMsg.AppendText(sourceItem.Code + ":" + ex.Message + "\r\n");
                    continue;
                }
            }
            ExcelHelper.WriteData(listData);
            txtMsg.AppendText("本次执行完成！！！\r\n");
            btnRun.Enabled = true;
        }

    }
}
