using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.InfoTest;
using TestDISC.Models.Report;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.UtilsProject;
using TestDISC.MServices.Interfaces;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class ResultDISCQueries : IResultDISCQueries
    {
        private readonly ITestDISCDapper _testDISCDapper;
        private readonly LinkBase _linkBase;

        public ResultDISCQueries(ITestDISCDapper testDISCDapper,
            IOptions<LinkBase> linkBase)
        {
            _testDISCDapper = testDISCDapper;
            _linkBase = linkBase.Value;
        }

        public async Task<ResultDISCModel> QueryResultDISC(OSearchResultDISC oSearchResultDISC)
        {
            oSearchResultDISC.id ??= 0;
            oSearchResultDISC.code ??= 0;
            oSearchResultDISC.codeText ??= "";

            var condition = "";

            if(oSearchResultDISC.id != 0)
            {
                condition += " and id = @id ";
            }

            if(oSearchResultDISC.code != 0)
            {
                condition += " and code = @code ";
            }

            if (!string.IsNullOrEmpty(oSearchResultDISC.codeText))
            {
                condition += " and codetext = @codetext ";
            }

            var query =
                @"select id, code, codetext, description, interpret, quanlyti 
                from resultdisc 
                where status = 1 "+ condition +@" ";

            return await _testDISCDapper.QuerySingleAsync<ResultDISCModel>(query, new
            {
                id = oSearchResultDISC.id,
                code = oSearchResultDISC.code,
                codetext = oSearchResultDISC.codeText,
            });
        }

        public async Task<IList<ResultDISCReportModel>> QueryReport(ReportFilter filter)
        {
            var condition = "";
            var conditionAnswer = "";

            if(filter.useranswerid > 0)
            {
                conditionAnswer += " and ua.id = @useranswerid ";
            }

            if(!string.IsNullOrWhiteSpace(filter.fromDate))
            {
                conditionAnswer += " and ua.createdate >= @fromDate ";
            }

            if (!string.IsNullOrWhiteSpace(filter.toDate))
            {
                conditionAnswer += " and ua.createdate <= @toDate ";
            }

            if (!string.IsNullOrWhiteSpace(filter.email))
            {
                condition += " and u.email like @email ";
            }

            if (!string.IsNullOrWhiteSpace(filter.phone))
            {
                condition += " and u.phone like @phone ";
            }

            if (filter.partnerid > 0)
            {
                condition += " and u.partnerid = @partnerid ";
            }

            if (!Utils.isTopSkills(filter.partnerviewid))
            {
                condition += " and u.partnerid = @partnerviewid ";
            }

            var query =
                @"select u.fullname, u.phone, u.email, u.namecompany, uar.codetext, uar.description, uar.interpret, 
	                uar.quanlyti, uar.createdate, concat('" + _linkBase.SelfLink + @"', 'Home/Result?useranswerid=', uar.id) linkresult,
                    p.name partnername, ifnull(u.jobposition, '') jobposition 
                from user u
                    inner join partner p on p.id = u.partnerid 
	                inner join (
		                select ua.id, ua.userid, ua.createdate, rd.codetext, rd.interpret, rd.quanlyti, rd.description 
                        from useranswer ua 
			                inner join resultdisc rd on ua.resultdiscid = rd.id 
                        where ua.status = 1 "+ conditionAnswer + @"
                            -- and ua.id in (
			                --     select max(id) useranswerid 
			                --     from useranswer  
			                --     where status = 1 
			                --     group by userid 
                            -- ) 
                    ) uar on u.id = uar.userid 
                where u.status = 1 " + condition + @" 
                order by uar.createdate desc ";

            return (await _testDISCDapper.QueryAsync<ResultDISCReportModel>(query, new
            {
                filter.useranswerid,
                filter.fromDate,
                filter.toDate,
                email = "%" + filter.email + "%",
                phone = "%" + filter.phone + "%",
                filter.partnerid,
                filter.partnerviewid,
            })).ToList();
        }

        public async Task<IList<PointAnswerModel>> QueryMostPoint(ulong useranswerid)
        {
            var query =
                @"select qd.mostpoint point, count(1) number 
                from useranswerquestiondetail uaqd 
	                inner join questiondetail qd on qd.id = uaqd.mostquestiondetailid  
                where uaqd.useranswerquestionid in (
	                select id 
	                from useranswerquestion 
	                where useranswerid = @useranswerid 
                ) 
                group by qd.mostpoint 
                order by qd.mostpoint ";

            return (await _testDISCDapper.QueryAsync<PointAnswerModel>(query, new
            {
                useranswerid
            })).ToList();
        }

        public async Task<IList<PointAnswerModel>> QueryLeastPoint(ulong useranswerid)
        {
            var query =
                @"select qd.leastpoint point, count(1) number 
                from useranswerquestiondetail uaqd 
	                inner join questiondetail qd on qd.id = uaqd.leastquestiondetailid  
                where uaqd.useranswerquestionid in (
	                select id 
	                from useranswerquestion 
	                where useranswerid = @useranswerid 
                ) 
                group by qd.leastpoint 
                order by qd.leastpoint ";

            return (await _testDISCDapper.QueryAsync<PointAnswerModel>(query, new
            {
                useranswerid
            })).ToList();
        }

        public Dictionary<string, string> GetMaxLeastPoint(IList<PointAnswerModel> leastList)
        {
            Dictionary<int, int> middlePairs = new Dictionary<int, int>();
            middlePairs.Add(1, 6);
            middlePairs.Add(2, 5);
            middlePairs.Add(3, 7);
            middlePairs.Add(4, 7);

            var mappings = InitMapping();

            string[] vDISC = new string[] { "", "D", "I", "S", "C" };

            string code = "";
            string codeText = "";

            if (leastList != null && leastList.Count > 0)
            {
                for (int index = 1; index < 6; index++)
                {
                    bool exists = leastList.Any(a => a.point == index);

                    if (exists == false)
                    {
                        leastList.Add(new PointAnswerModel()
                        {
                            point = index,
                            number = 0,
                        });
                    }
                }

                leastList = leastList.Where(x => x.point != 5).ToList();

                foreach (var item in leastList)
                {
                    if (item.number < middlePairs[item.point])
                    {
                        item.index = Find(mappings, item);
                    }
                    else
                    {
                        item.index = 99;
                    }
                }

                leastList = leastList.Where(x => x.point != 5)
                    .OrderBy(a => a.index)
                    .ThenBy(a => a.point)
                    .ToList();

                foreach (var item in leastList)
                {
                    if (item.number < middlePairs[item.point])
                    {
                        code += item.point;
                        codeText += vDISC[item.point];

                        if(code.Trim().Length > 2)
                        {
                            break;
                        }
                    }
                }
            }

            Dictionary<string, string> codePairs = new Dictionary<string, string>
            {
                { "code", code },
                { "codeText", codeText }
            };

            return codePairs;
        }

        public (InfoTestModel, string, Useranswer) CheckValidAnswer(InfoTestModel infoTest)
        {
            var questions = infoTest.QuestionGroup.Questions;

            if(questions != null && questions.Count > 0)
            {
                var temp = questions.Select((a, i) => new { item = a, index = i });

                //Tìm xem có câu nào chưa đánh
                var itemError = temp.Where(a => a.item.mostchoosenid == 0 || a.item.leastchoosenid == 0).FirstOrDefault();

                if(itemError != null)
                {
                    infoTest.QuestionGroup.ActiveQuestion = itemError.index;

                    return (infoTest, "Bạn chưa trả lời câu hỏi " + (itemError.index + 1) + ".", null);
                }

                //Tìm xem có câu nào bị trùng nhau
                itemError = temp.Where(a => a.item.mostchoosenid == a.item.leastchoosenid)?.FirstOrDefault();

                if (itemError != null)
                {
                    infoTest.QuestionGroup.ActiveQuestion = itemError.index;

                    return (infoTest, "Câu trả lời cho câu hỏi " + (itemError.index + 1) + " bị trùng nhau.", null);
                }
            }
            else
            {
                //Không điền câu hỏi thì quay lại từ đầu
                infoTest.QuestionGroup.ActiveQuestion = 0;

                return (infoTest, "Vui lòng điền đầy đủ các câu hỏi.", null);
            }

            return (infoTest, "", null);
        }

        public void ExportReportSheets(ExcelPackage package, IList<ResultDISCReportModel> resultDISCReports)
        {
            ExcelWorksheet Sheet = package.Workbook.Worksheets.Add("Report");

            Sheet.Cells["A1:L1"].Style.Font.Bold = true;
            Sheet.Cells["A1:L1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            Sheet.Cells["A1:L1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            Sheet.Cells.Style.WrapText = true;
            Sheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            Sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            Sheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            Sheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            Sheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            Sheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            Sheet.Cells.AutoFitColumns();

            Sheet.Column(1).Width = 25;
            Sheet.Column(2).Width = 15;
            Sheet.Column(3).Width = 25;
            Sheet.Column(4).Width = 25;
            Sheet.Column(5).Width = 25;
            Sheet.Column(6).Width = 25;
            Sheet.Column(7).Width = 15;
            Sheet.Column(8).Width = 45;
            Sheet.Column(9).Width = 45;
            Sheet.Column(10).Width = 45;
            Sheet.Column(11).Width = 20;
            Sheet.Column(12).Width = 30;

            Sheet.Cells[1, 1].Value = "Họ tên";
            Sheet.Cells[1, 2].Value = "Điện thoại";
            Sheet.Cells[1, 3].Value = "Email";
            Sheet.Cells[1, 4].Value = "Tên công ty";
            Sheet.Cells[1, 5].Value = "Vị trí công việc";
            Sheet.Cells[1, 6].Value = "Đối tác";
            Sheet.Cells[1, 7].Value = "Kết quả DISC";
            Sheet.Cells[1, 8].Value = "Mô tả";
            Sheet.Cells[1, 9].Value = "Nhóm công việc";
            Sheet.Cells[1, 10].Value = "Công việc";
            Sheet.Cells[1, 11].Value = "Thời gian test";
            Sheet.Cells[1, 12].Value = "Link kết quả";

            int i = 1;
            int row = 2;

            foreach (var item in resultDISCReports)
            {
                Sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                Sheet.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Sheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                Sheet.Cells[row, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                Sheet.Cells[row, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                Sheet.Cells[row, 1].Value = item.fullname;
                Sheet.Cells[row, 2].Value = item.phone;
                Sheet.Cells[row, 3].Value = item.email;
                Sheet.Cells[row, 4].Value = item.namecompany;
                Sheet.Cells[row, 5].Value = item.jobposition;
                Sheet.Cells[row, 6].Value = item.partnername;
                Sheet.Cells[row, 7].Value = item.codetext;
                Sheet.Cells[row, 8].Value = item.description;
                Sheet.Cells[row, 9].Value = item.interpret;
                Sheet.Cells[row, 10].Value = item.quanlyti;
                Sheet.Cells[row, 11].Value = item.createdate.ToString("dd-MM-yyyy HH:mm");

                //Link
                Sheet.Cells[row, 12].Hyperlink = new Uri(item.linkresult, UriKind.Absolute);
                Sheet.Cells[row, 12].Value = item.linkresult;
                Sheet.Cells[row, 12].Style.Font.UnderLine = true;
                Sheet.Cells[row, 12].Style.Font.Color.SetColor(Color.Blue);

                row++;
                i++;
            }
        }

        //Private
        private List<MappingOnTable> InitMapping()
        {
            var listMapping = new List<MappingOnTable>
            {
                new MappingOnTable(0, 0, 0, 0),
                new MappingOnTable(-1, -1, 1, 1),
                new MappingOnTable(1, -1, -1, -1),
                new MappingOnTable(-1, 1, 2, -1),
                new MappingOnTable(-1, -1, -1, 2),
                new MappingOnTable(2, -1, -1, -1),
                new MappingOnTable(-1, 2, 3, 3),
                new MappingOnTable(3, -1, -1, -1),
                new MappingOnTable(-1, 3, 4, 4),
                new MappingOnTable(4, -1, -1, -1),
                new MappingOnTable(-1, -1, 5, 5),
                new MappingOnTable(5, 4, -1, -1),
                new MappingOnTable(-1, -1, 6, 6)
            };

            return listMapping;
        }

        private int Find(List<MappingOnTable> mappings, PointAnswerModel pointAnswer)
        {
            int index = 0;

            foreach (var item in mappings)
            {
                if (pointAnswer.point == 1 && item.D == pointAnswer.number)
                {
                    return index;
                }
                else if (pointAnswer.point == 2 && item.I == pointAnswer.number)
                {
                    return index;
                }
                else if (pointAnswer.point == 3 && item.S == pointAnswer.number)
                {
                    return index;
                }
                else if (pointAnswer.point == 4 && item.C == pointAnswer.number)
                {
                    return index;
                }

                index++;
            }

            return 99;
        }
    }
}
