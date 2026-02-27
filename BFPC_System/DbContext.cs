using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BPFC_System;

public class BpfcDbContext
{
    private readonly string connectionString;

    public BpfcDbContext()
    {
        connectionString = ConfigHelper.GetConnectionString("strCon");
    }

    public BpfcDbContext(string connectionString)
    {
        this.connectionString = connectionString;
    }


    public List<ResultViewModel> GetResults(DateTime dtpReportDate)
    {
        List<ResultViewModel> results = new List<ResultViewModel>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
        SELECT 
            P.LineID, 
            P.LineName,
            A.Model,
            A.ArticleName,
            AP.PartName,
            TempR.StandardTemp_Heat,
            TempR.StandardTemp_1,
            TempR.StandardTemp_2,
            TempR.StandardTemp_3,
            TempR.ActualTemp_Heat,
            TempR.ActualTemp_1,
            TempR.ActualTemp_2,
            TempR.ActualTemp_3,
            TempR.Result_Heat AS ResultTemp_Heat,
            TempR.Result_1 AS ResultTemp_1,
            TempR.Result_2 AS ResultTemp_2,
            TempR.Result_3 AS ResultTemp_3,
            TR.StandardTime_Heat,
            TR.StandardTime_1,
            TR.StandardTime_2,
            TR.StandardTime_3,
            TR.ActualTime_Heat,
            TR.ActualTime_1,
            TR.ActualTime_2,
            TR.ActualTime_3,
            TR.Result_Heat AS ResultTime_Heat,
            TR.Result_1 AS ResultTime_1,
            TR.Result_2 AS ResultTime_2,
            TR.Result_3 AS ResultTime_3,
            CR.StandardChemical_1,
            CR.StandardChemical_2,
            CR.StandardChemical_3,
            CR.ActualChemical_1,
            CR.ActualChemical_2,
            CR.ActualChemical_3,
            CR.Result_1 AS ResultChemical_1,
            CR.Result_2 AS ResultChemical_2,
            CR.Result_3 AS ResultChemical_3
        FROM ProductionLines P
        JOIN ChemicalResults CR ON P.LineID = CR.LineID
        JOIN ArticleParts AP ON CR.ArticlePartID = AP.PartID
        JOIN Articles A ON AP.ArticleID = A.ArticleID
        JOIN TimeResults TR ON P.LineID = TR.LineID AND CR.ArticlePartID = TR.ArticlePartID
        JOIN TemperatureResults TempR ON P.LineID = TempR.LineID AND CR.ArticlePartID = TempR.ArticlePartID
        WHERE 
            CONVERT(DATE, CR.ReportDate) = @ReportDate
            AND CONVERT(DATE, TR.ReportDate) = @ReportDate
            AND CONVERT(DATE, TempR.ReportDate) = @ReportDate
        ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ReportDate", dtpReportDate.Date);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ResultViewModel result = new ResultViewModel
                        {
                            LineID = (int)reader["LineID"],
                            LineName = (string)reader["LineName"],
                            Model = (string)reader["Model"],
                            ArticleName = (string)reader["ArticleName"],
                            PartName = (string)reader["PartName"],

                            ResultTemp_Heat = reader["ResultTemp_Heat"] == DBNull.Value ? null : (string)reader["ResultTemp_Heat"],
                            ResultTemp_1 = reader["ResultTemp_1"] == DBNull.Value ? null : (string)reader["ResultTemp_1"],
                            ResultTemp_2 = reader["ResultTemp_2"] == DBNull.Value ? null : (string)reader["ResultTemp_2"],
                            ResultTemp_3 = reader["ResultTemp_3"] == DBNull.Value ? null : (string)reader["ResultTemp_3"],

                            ActualTime_Heat = reader["ActualTime_Heat"] == DBNull.Value ? null : (string)reader["ActualTime_Heat"],
                            ActualTime_1 = reader["ActualTime_1"] == DBNull.Value ? null : (string)reader["ActualTime_1"],
                            ActualTime_2 = reader["ActualTime_2"] == DBNull.Value ? null : (string)reader["ActualTime_2"],
                            ActualTime_3 = reader["ActualTime_3"] == DBNull.Value ? null : (string)reader["ActualTime_3"],

                            StandardTime_Heat = reader["StandardTime_Heat"] == DBNull.Value ? null : (string)reader["StandardTime_Heat"],
                            StandardTime_1 = reader["StandardTime_1"] == DBNull.Value ? null : (string)reader["StandardTime_1"],
                            StandardTime_2 = reader["StandardTime_2"] == DBNull.Value ? null : (string)reader["StandardTime_2"],
                            StandardTime_3 = reader["StandardTime_3"] == DBNull.Value ? null : (string)reader["StandardTime_3"],

                            ResultTime_Heat = reader["ResultTime_Heat"] == DBNull.Value ? null : (string)reader["ResultTime_Heat"],
                            ResultTime_1 = reader["ResultTime_1"] == DBNull.Value ? null : (string)reader["ResultTime_1"],
                            ResultTime_2 = reader["ResultTime_2"] == DBNull.Value ? null : (string)reader["ResultTime_2"],
                            ResultTime_3 = reader["ResultTime_3"] == DBNull.Value ? null : (string)reader["ResultTime_3"],

                            ActualChemical_1 = reader["ActualChemical_1"] == DBNull.Value ? null : (string)reader["ActualChemical_1"],
                            ActualChemical_2 = reader["ActualChemical_2"] == DBNull.Value ? null : (string)reader["ActualChemical_2"],
                            ActualChemical_3 = reader["ActualChemical_3"] == DBNull.Value ? null : (string)reader["ActualChemical_3"],

                            StandardChemical_1 = reader["StandardChemical_1"] == DBNull.Value ? null : (string)reader["StandardChemical_1"],
                            StandardChemical_2 = reader["StandardChemical_2"] == DBNull.Value ? null : (string)reader["StandardChemical_2"],
                            StandardChemical_3 = reader["StandardChemical_3"] == DBNull.Value ? null : (string)reader["StandardChemical_3"],

                            ResultChemical_1 = reader["ResultChemical_1"] == DBNull.Value ? null : (string)reader["ResultChemical_1"],
                            ResultChemical_2 = reader["ResultChemical_2"] == DBNull.Value ? null : (string)reader["ResultChemical_2"],
                            ResultChemical_3 = reader["ResultChemical_3"] == DBNull.Value ? null : (string)reader["ResultChemical_3"],

                            ActualTemp_Heat = HandleFloatDBNull(reader, "ActualTemp_Heat"),
                            ActualTemp_1 = HandleFloatDBNull(reader, "ActualTemp_1"),
                            ActualTemp_2 = HandleFloatDBNull(reader, "ActualTemp_2"),
                            ActualTemp_3 = HandleFloatDBNull(reader, "ActualTemp_3"),

                            StandardTemp_Heat = HandleFloatDBNull(reader, "StandardTemp_Heat"),
                            StandardTemp_1 = HandleFloatDBNull(reader, "StandardTemp_1"),
                            StandardTemp_2 = HandleFloatDBNull(reader, "StandardTemp_2"),
                            StandardTemp_3 = HandleFloatDBNull(reader, "StandardTemp_3"),
                        };

                    results.Add(result);
                    }
                }
            }
        }

        return results;
    }

    private float? HandleFloatDBNull(SqlDataReader reader, string columnName)
    {
        if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
        {
            float value;
            if (float.TryParse(reader[columnName].ToString(), out value))
            {
                return value;
            }
        }

        return null;
    }

    public List<ResultViewModel> GetAtucaChemicalForLineAndDate(string lineName, DateTime selectedDate)
    {
        List<ResultViewModel> results = new List<ResultViewModel>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
            SELECT 
                P.LineID,  
                P.LineName,
                A.Model,
                A.ArticleName,
                AP.PartName,
                AP.StandardChemical_1,
                AP.StandardChemical_2,
                AP.StandardChemical_3,
                CR.ActualChemical_1,
                CR.Result_1 AS ResultChemical_1,
                CR.ActualChemical_2,
                CR.Result_2 AS ResultChemical_2,
                CR.ActualChemical_3,
                CR.Result_3 AS ResultChemical_3
            FROM ProductionLines P
            JOIN ChemicalResults CR ON P.LineID = CR.LineID
            JOIN ArticleParts AP ON CR.ArticlePartID = AP.PartID
            JOIN Articles A ON AP.ArticleID = A.ArticleID
            WHERE 
                P.LineName = @LineName
                AND CONVERT(DATE, CR.ReportDate) = @SelectedDate
        ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                command.Parameters.AddWithValue("@LineName", lineName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ResultViewModel result = new ResultViewModel
                        {
                            LineID = (int)reader["LineID"],
                            LineName = (string)reader["LineName"],
                            Model = (string)reader["Model"],
                            ArticleName = (string)reader["ArticleName"],
                            PartName = (string)reader["PartName"],

                            ActualChemical_1 = reader["ActualChemical_1"] == DBNull.Value ? null : (string)reader["ActualChemical_1"],
                            ActualChemical_2 = reader["ActualChemical_2"] == DBNull.Value ? null : (string)reader["ActualChemical_2"],
                            ActualChemical_3 = reader["ActualChemical_3"] == DBNull.Value ? null : (string)reader["ActualChemical_3"],

                            StandardChemical_1 = reader["StandardChemical_1"] == DBNull.Value ? null : (string)reader["StandardChemical_1"],
                            StandardChemical_2 = reader["StandardChemical_2"] == DBNull.Value ? null : (string)reader["StandardChemical_2"],
                            StandardChemical_3 = reader["StandardChemical_3"] == DBNull.Value ? null : (string)reader["StandardChemical_3"],

                            ResultChemical_1 = reader["ResultChemical_1"] == DBNull.Value ? null : (string)reader["ResultChemical_1"],
                            ResultChemical_2 = reader["ResultChemical_2"] == DBNull.Value ? null : (string)reader["ResultChemical_2"],
                            ResultChemical_3 = reader["ResultChemical_3"] == DBNull.Value ? null : (string)reader["ResultChemical_3"],
                        };

                        results.Add(result);
                    }
                }
            }
        }

        return results;
    }



    public List<ResultViewModel> GetAtucalForLineAndDate(string lineName, DateTime selectedDate)
    {
        List<ResultViewModel> results = new List<ResultViewModel>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
            SELECT 
                P.LineID, 
                P.LineName,
                A.Model,
                A.ArticleName,
                AP.PartName,
                AP.StandardTemp_1,
                AP.StandardTemp_2,
                AP.StandardTemp_3,
                TempR.ActualTemp_1,
                TempR.Result_1 AS ResultTemp_1,
                TempR.ActualTemp_2,
                TempR.Result_2 AS ResultTemp_2,
                TempR.ActualTemp_3,
                TempR.Result_3 AS ResultTemp_3,
                AP.StandardTime_1,
                TR.ActualTime_1,
                TR.Result_1 AS ResultTime_1,
                AP.StandardTime_2,
                TR.ActualTime_2,
                TR.Result_2 AS ResultTime_2,
                AP.StandardTime_3,
                TR.ActualTime_3,
                TR.Result_3 AS ResultTime_3
            FROM ProductionLines P
            JOIN TimeResults TR ON P.LineID = TR.LineID
            JOIN TemperatureResults TempR ON P.LineID = TempR.LineID
            JOIN ArticleParts AP ON TR.ArticlePartID = AP.PartID AND TempR.ArticlePartID = AP.PartID
            JOIN Articles A ON AP.ArticleID = A.ArticleID
            WHERE 
                P.LineName = @LineName
                AND CONVERT(DATE, TR.ReportDate) = @SelectedDate
                AND CONVERT(DATE, TempR.ReportDate) = @SelectedDate;
        ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                command.Parameters.AddWithValue("@LineName", lineName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ResultViewModel result = new ResultViewModel
                        {
                            LineID = (int)reader["LineID"],
                            LineName = (string)reader["LineName"],
                            Model = (string)reader["Model"],
                            ArticleName = (string)reader["ArticleName"],
                            PartName = (string)reader["PartName"],

                            ResultTemp_1 = reader["ResultTemp_1"] == DBNull.Value ? null : (string)reader["ResultTemp_1"],
                            ResultTemp_2 = reader["ResultTemp_2"] == DBNull.Value ? null : (string)reader["ResultTemp_2"],
                            ResultTemp_3 = reader["ResultTemp_3"] == DBNull.Value ? null : (string)reader["ResultTemp_3"],

                            ActualTime_1 = reader["ActualTime_1"] == DBNull.Value ? null : (string)reader["ActualTime_1"],
                            ActualTime_2 = reader["ActualTime_2"] == DBNull.Value ? null : (string)reader["ActualTime_2"],
                            ActualTime_3 = reader["ActualTime_3"] == DBNull.Value ? null : (string)reader["ActualTime_3"],

                            StandardTime_1 = reader["StandardTime_1"] == DBNull.Value ? null : (string)reader["StandardTime_1"],
                            StandardTime_2 = reader["StandardTime_2"] == DBNull.Value ? null : (string)reader["StandardTime_2"],
                            StandardTime_3 = reader["StandardTime_3"] == DBNull.Value ? null : (string)reader["StandardTime_3"],

                            ResultTime_1 = reader["ResultTime_1"] == DBNull.Value ? null : (string)reader["ResultTime_1"],
                            ResultTime_2 = reader["ResultTime_2"] == DBNull.Value ? null : (string)reader["ResultTime_2"],
                            ResultTime_3 = reader["ResultTime_3"] == DBNull.Value ? null : (string)reader["ResultTime_3"],

                            ActualTemp_1 = HandleFloatDBNull(reader, "ActualTemp_1"),
                            ActualTemp_2 = HandleFloatDBNull(reader, "ActualTemp_2"),
                            ActualTemp_3 = HandleFloatDBNull(reader, "ActualTemp_3"),

                            StandardTemp_1 = HandleFloatDBNull(reader, "StandardTemp_1"),
                            StandardTemp_2 = HandleFloatDBNull(reader, "StandardTemp_2"),
                            StandardTemp_3 = HandleFloatDBNull(reader, "StandardTemp_3"),
                        };

                        results.Add(result);
                    }
                }
            }
        }

        return results;
    }


    public List<(string LineName, int LineID)> GetLineNamesWithID()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT LineName, LineID FROM ProductionLines";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<(string LineName, int LineID)> lines = new List<(string LineName, int LineID)>();
                    while (reader.Read())
                    {
                        string lineName = reader["LineName"].ToString();
                        int lineID = Convert.ToInt32(reader["LineID"]);
                        lines.Add((lineName, lineID));
                    }
                    return lines;
                }
            }
        }
    }

    public class ResultViewModel
    {
        public int LineID { get; set; }
        public string LineName { get; set; }
        public string Model { get; set; }
        public string ArticleName { get; set; }
        public string PartName { get; set; }

        public float? StandardTemp_Heat { get; set; }
        public float? ActualTemp_Heat { get; set; }
        public string ResultTemp_Heat { get; set; }

        public string StandardTime_Heat { get; set; }
        public string ActualTime_Heat { get; set; }
        public string ResultTime_Heat { get; set; }

        public float? StandardTemp_1 { get; set; }
        public float? ActualTemp_1 { get; set; }
        public string ResultTemp_1 { get; set; }

        public float? StandardTemp_2 { get; set; }
        public float? ActualTemp_2 { get; set; }
        public string ResultTemp_2 { get; set; }

        public float? StandardTemp_3 { get; set; }
        public float? ActualTemp_3 { get; set; }
        public string ResultTemp_3 { get; set; }

        public string StandardTime_1 { get; set; }
        public string ActualTime_1 { get; set; }
        public string ResultTime_1 { get; set; }

        public string StandardTime_2 { get; set; }
        public string ActualTime_2 { get; set; }
        public string ResultTime_2 { get; set; }

        public string StandardTime_3 { get; set; }
        public string ActualTime_3 { get; set; }
        public string ResultTime_3 { get; set; }

        public string StandardChemical_1 { get; set; }
        public string ActualChemical_1 { get; set; }
        public string ResultChemical_1 { get; set; }
        public string StandardChemical_2 { get; set; }
        public string ActualChemical_2 { get; set; }
        public string ResultChemical_2 { get; set; }
        public string StandardChemical_3 { get; set; }
        public string ActualChemical_3 { get; set; }
        public string ResultChemical_3 { get; set; }
    }
}
