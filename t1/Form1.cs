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
                    string initialDirectory = System.Environment.CurrentDirectory;
                    //openFileDialog.InitialDirectory = @"C:\Data";
                    openFileDialog.InitialDirectory = initialDirectory;
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
                            model.Company = row[0].ToString().Trim();
                            model.Huowei = row[1].ToString().Trim();
                            model.ItemCode = row[2].ToString().Trim();
                            model.Count = Convert.ToInt32(row[3]);
                            model.OldDate = row[4].ToString().Trim();
                            model.NewData = row[5].ToString().Trim();
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
                        string oldDate = string.IsNullOrWhiteSpace(sourceItem.OldDate) ? null : sourceItem.OldDate;

                        DbSet<LOCATION_INVENTORY> LocationInventory = ctx.Set<LOCATION_INVENTORY>();

                        var locations = LocationInventory.Where(l =>
                          l.COMPANY == sourceItem.Company
                          && l.ITEM == sourceItem.ItemCode
                          && l.LOCATION == sourceItem.Huowei
                          && l.ATTRIBUTE3 == sourceItem.OldDate
                          && l.ON_HAND_QTY == sourceItem.Count
                        );

                        if (locations.Count() > 1)
                        {
                            sourceItem.Msg = "数据源匹配到多条数据";
                            txtMsg.AppendText(sourceItem.ItemCode + ":" + sourceItem.Msg + "\r\n");
                            continue;
                        }

                        if (!locations.Any())
                        {
                            sourceItem.Msg = "未匹配到数据,请检查数量、编码、日期、货位";
                            txtMsg.AppendText(sourceItem.ItemCode + ":" + sourceItem.Msg + "\r\n");
                            continue;
                        }

                        foreach (var loc in locations)
                        {
                            //if (loc.ATTRIBUTE3 != sourceItem.NewData)
                            //{

                            var attr = ctx.ATTRIBUTE.First(a => a.COMPANY == loc.COMPANY && a.ITEM == loc.ITEM && a.ATTRIBUTE3 == loc.ATTRIBUTE3);
                            if (attr != null)
                            {
                                loc.ATTRIBUTE_NUM = attr.ATTRIBUTE_NUM;
                            }
                            else
                            {
                                var newAttribute = new ATTRIBUTE
                                {
                                    ITEM = loc.ITEM,
                                    COMPANY = loc.COMPANY,
                                    ATTRIBUTE3 = sourceItem.NewData,
                                    USER_STAMP = "system",
                                    DATE_TIME_STAMP = DateTime.Now
                                };

                                using (var ctx1 = new SCVEntities())
                                {
                                    ctx1.ATTRIBUTE.Add(newAttribute);
                                    ctx1.SaveChanges();
                                }

                                loc.ATTRIBUTE_NUM = newAttribute.ATTRIBUTE_NUM;
                            }

                            loc.ATTRIBUTE3 = sourceItem.NewData;


                            var locationsA = LocationInventory.Where(l =>
                             l.COMPANY == loc.COMPANY
                             && l.ITEM == loc.ITEM
                             && l.LOCATION == loc.LOCATION
                             && l.ATTRIBUTE3 == loc.ATTRIBUTE3
                             && l.INVENTORY_STS == loc.INVENTORY_STS
                             && l.ATTRIBUTE_NUM == loc.ATTRIBUTE_NUM
                           );

                            var locF = locationsA.First();
                            if (locF.INTERNAL_LOCATION_INV != loc.INTERNAL_LOCATION_INV)
                            {
                                if (loc.IN_TRANSIT_QTY == 0 && loc.ALLOCATED_QTY == 0) {
                                    locF.ON_HAND_QTY += loc.ON_HAND_QTY;
                                    LocationInventory.Remove(loc);
                                }
                            }

                            sourceItem.Msg = "修改成功";
                            //}
                            //else
                            //{
                            //    sourceItem.Msg = "新效期与原效期相同，无需修改";
                            //}
                        }
                        ctx.SaveChanges();
                        txtMsg.AppendText(sourceItem.ItemCode + ":" + sourceItem.Msg + "\r\n");
                    }
                }
                catch (Exception ex)
                {
                    sourceItem.Msg = ex.Message;
                    txtMsg.AppendText(sourceItem.ItemCode + ":" + ex.Message + "\r\n");
                    continue;
                }
            }
            ExcelHelper.WriteData(listData);
            txtMsg.AppendText("本次执行完成！！！\r\n");
            btnRun.Enabled = true;
        }

    }
}
