using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

// MODEL
// - PROPERTIES
//   - 61 ..
// - ERD
//   - SUBMODELS
//   - DICTTYPES
//   - ENTITIES
//   - TRIGGERS
//   - TEXTOBJECTS
//   - USERS
//   - USERROLES
//   - USERROLEUSERS
//   - USERPERMISSIONS
//   - TEXTES
//   - STAMPS
//   - LISTTRIGGERS
// - DFD
//   - 23 ..

namespace mdlr
{
    public static class ImportExport
    {
        #region support

        private class Paths
        {
            public const string Properties = "/MODEL/PROPERTIES/";
            public const string ERD = "/MODEL/ERD/";
        }

        private class DataType
        {
            public string Name { get; set; }
            public string Default { get; set; }
            public string Check { get; set; }

            public DataType(string name, string @default = null, string check = null)
            {
                this.Name = name;
                this.Default = @default;
                this.Check = check;
            }
        }

        private static readonly Dictionary<string, DataType> dictDataTypes = new Dictionary<string, DataType>() {
            {  "2", new DataType("TimeDate") },
            {  "3", new DataType("TWeight") },
            {  "6", new DataType("BooleanF")/*, "0", "%colname% BETWEEN 0 AND 1")*/ },
            {  "7", new DataType("BooleanT") },
            {  "8", new DataType("TMemo") },
            {  "9", new DataType("TNameUTF") },
            { "10", new DataType("TLongInt") },
            { "11", new DataType("TIMG") },
            { "12", new DataType("TNameL") },
            { "13", new DataType("TNameDef") },
            { "14", new DataType("TSmallInt") },
            { "15", new DataType("TLargeMemoUTF") }, // 2
            { "17", new DataType("TMemoUTF") },      // 1
            { "20", new DataType("TBinaryData") },
            { "21", new DataType("TFloat") },
            { "23", new DataType("TimeDateUTC") },
            { "24", new DataType("TColor") },
            { "25", new DataType("TShortText") },
            { "26", new DataType("RDB$RELATION_NAME") },
            { "27", new DataType("RDB$FIELD_NAME") },
            { "28", new DataType("TMemoUTF") },      // 1
            { "29", new DataType("RDB$GENERATOR_NAME") },
            { "30", new DataType("TLargeMemoUTF") }, // 2
            { "31", new DataType("Int64") },
            { "32", new DataType("TNameLUTF") },
            { "33", new DataType("TEmail"/*, check: "%colname% IS NULL OR %colname% SIMILAR TO '%_@_%'"*/) },
            { "34", new DataType("TCurrencyName") },
            { "35", new DataType("TName163UTF") },
            { "36", new DataType("TCurrency") }
        };

        private static readonly Dictionary<string, string> dataTypes = new Dictionary<string, string>()
        {
            { "10", "Char" },
            { "20", "Varchar" },
            { "30", "Smallint" },
            { "40", "Integer" },
            { "50", "Float" },
            { "60", "Double precision" },
            { "70", "Date" },
            { "76", "Timestamp" },
            { "80", "blob" },
            { "100", "Numeric" }
        };

        public static string LoadAndGenerateSql(string text)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(text);

            return GenerateSql(xDoc);
        }

        public static DateTime GetDateTime(this XmlDocument xDoc, string xPath)
        {
            var nd = xDoc.SelectSingleNode(xPath);
            DateTime.TryParse(nd.InnerText, out DateTime dt);
            return dt;
        }

        public static string GetString(this XmlDocument xDoc, string xPath)
        {
            var nd = xDoc.SelectSingleNode(xPath);
            return nd.InnerText;
        }

        #endregion support

        public static string GenerateSql(XmlDocument xDoc)
        {
            var sb = new StringBuilder();

            #region header

            sb.AppendLine("/*");

            var dt = xDoc.GetDateTime($"{Paths.Properties}CREATED/DATE");
            sb.AppendLine($"Created\t\t{dt.ToString("yyyy-MM-dd")}");

            dt = xDoc.GetDateTime($"{Paths.Properties}MODIFIED/DATE");
            sb.AppendLine($"Modified\t\t{dt.ToString("yyyy-MM-dd")}");

            var str = xDoc.GetString($"{Paths.Properties}PROJECTNAME");
            sb.AppendLine($"Project\t\t{str}");

            str = xDoc.GetString($"{Paths.Properties}MODELNAME");
            sb.AppendLine($"Model\t\t\t{str}");

            str = xDoc.GetString($"{Paths.Properties}COMPANY");
            sb.AppendLine($"Company\t\t{str}");

            str = xDoc.GetString($"{Paths.Properties}AUTHOR");
            sb.AppendLine($"Author\t\t{str}");

            str = xDoc.GetString($"{Paths.Properties}VERSION");
            sb.AppendLine($"Version\t\t{str}");

            sb.AppendLine("*/");

            #endregion header

            sb.AppendLine();
            sb.AppendLine();

            str = xDoc.GetString($"{Paths.Properties}SCRIPTBEFORE");
            sb.AppendLine(str);
            sb.AppendLine();

            #region domains

            string getDomain(string idDataType, string length, string dcimal)
            {
                switch (idDataType)
                {
                    case "10":
                        return $"{dataTypes[idDataType]}({length})";

                    case "20":
                        return $"{dataTypes[idDataType]}({length})";

                    case "50":
                        return $"{dataTypes[idDataType]}";

                    case "60":
                        return $"{dataTypes[idDataType]}";

                    case "76":
                        return $"{dataTypes[idDataType]}";

                    case "80":
                        return $"{dataTypes[idDataType]} sub_type {dcimal} segment size {length}";

                    case "100":
                        return $"{dataTypes[idDataType]}({length},{dcimal})";

                    default:
                        throw new NotImplementedException("Unknown DATATYPEID: " + idDataType);
                }
            }

            foreach (XmlNode node in xDoc.SelectSingleNode($"{Paths.ERD}DICTTYPES").ChildNodes)
            {
                var udt = node.SelectSingleNode("USERDATATYPE");
                var id = node.SelectSingleNode("ID").InnerText;
                var dataTypeId = node.SelectSingleNode("DATATYPEID").InnerText;
                var name = node.SelectSingleNode("DICTNAME").InnerText;
                var check = node.SelectSingleNode("CHECK").InnerText;
                var dfault = node.SelectSingleNode("DEFAULT").InnerText;

                DataType type = null;
                if (!dictDataTypes.ContainsKey(id))
                {
                    type = new DataType(name);
                    dictDataTypes.Add(id, type);
                }
                else
                    type = dictDataTypes[id];

                type.Default = dfault;
                type.Check = check;

                if (!string.IsNullOrWhiteSpace(udt.InnerText))
                {
                    sb.AppendLine();
                    continue;
                }

                sb.Append("CREATE DOMAIN ");
                sb.Append(name);
                sb.Append(" AS ");

                sb.Append(getDomain(dataTypeId, node.SelectSingleNode("LENGTH").InnerText, node.SelectSingleNode("DECIMAL").InnerText));

                var def = node.SelectSingleNode("DEF").InnerText;
                if (!string.IsNullOrWhiteSpace(def))
                    sb.Append($" {def}");

                if (!string.IsNullOrWhiteSpace(check))
                    sb.Append($" CHECK ({check.Replace("%colname%", "value")})");

                if (!string.IsNullOrWhiteSpace(dfault))
                    sb.Append(" DEFAULT " + dfault);

                sb.Append(";" + Environment.NewLine);
            }

            #endregion domains

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();

            #region entities

            string getDataType(string idDictionaryDataType, string idDataType, string length, string dcimal, out string typeDefault, out string typeCheck)
            {
                typeCheck = typeDefault = null;
                string result = null;

                if (idDictionaryDataType != "0")
                {
                    var type = dictDataTypes[idDictionaryDataType];
                    result = type.Name;
                    typeDefault = type.Default;
                    typeCheck = type.Check;
                }

                if (idDataType != "0")
                {
                    if (idDataType == "200") return string.Empty; // computed field - no explicit domain

                    result = dataTypes[idDataType];
                }

                if (string.IsNullOrWhiteSpace(result))
                    throw new NotImplementedException("Unknown attribute data type definition");

                switch (result)
                {
                    case "Char":
                    case "Varchar":
                        result = $"{result}({length})";
                        break;

                    case "blob":
                        result = $"{result} sub_type {dcimal} segment size {length}";
                        break;

                    case "Numeric":
                        result = $"{result}({length},{dcimal})";
                        break;
                }

                return result;
            };

            string getConstraint(bool trim, string constraintDefinition, string tableName, string columnName)
            {
                if (string.IsNullOrWhiteSpace(constraintDefinition))
                    return string.Empty;

                if (constraintDefinition.IndexOf("%tablename%", StringComparison.OrdinalIgnoreCase) >= 0)
                    constraintDefinition = Regex.Replace(constraintDefinition, "%tablename%", tableName, RegexOptions.IgnoreCase);

                if (constraintDefinition.IndexOf("%table%", StringComparison.OrdinalIgnoreCase) >= 0)
                    constraintDefinition = Regex.Replace(constraintDefinition, "%table%", "_table_", RegexOptions.IgnoreCase); // TODO masa$updates.minSystemNumber - is this ok?

                if (constraintDefinition.IndexOf("%colname%", StringComparison.OrdinalIgnoreCase) >= 0)
                    constraintDefinition = Regex.Replace(constraintDefinition, "%colname%", columnName, RegexOptions.IgnoreCase);

                if (string.IsNullOrWhiteSpace(constraintDefinition))
                    throw new NotImplementedException("Unknow pk constraint: " + constraintDefinition);

                if (trim && constraintDefinition.Length > 31)
                    constraintDefinition = constraintDefinition.Substring(0, 31);

                return constraintDefinition;
            };

            var entities = xDoc.SelectSingleNode($"{Paths.ERD}ENTITIES").ChildNodes;

            foreach (XmlNode node in entities)
            {
                string tableName = node.SelectSingleNode("NAME").InnerText;

                sb.AppendLine($"Create Table {tableName}  (");

                var primaryKeys = new List<string>();

                var pkConstraint = getConstraint(true, node.SelectSingleNode("PKCONSTRAINT").InnerText, tableName, null);
                if (!string.IsNullOrWhiteSpace(pkConstraint)) pkConstraint = "Constraint " + pkConstraint;

                foreach (XmlNode attr in node.SelectSingleNode("ATTRIBUTES").ChildNodes)
                {
                    string columnName = attr.SelectSingleNode("COLNAME").InnerText;

                    var dataType = getDataType(attr.SelectSingleNode("DICTTYPEID").InnerText, attr.SelectSingleNode("DATATYPEID").InnerText, attr.SelectSingleNode("LENGTH").InnerText, attr.SelectSingleNode("DECIMAL").InnerText,
                        out string typeDefault, out string typeCheck);

                    var dfault = attr.SelectSingleNode("DEFAULT").InnerText;

                    if (dfault == typeDefault)
                        dfault = null;

                    if (!string.IsNullOrWhiteSpace(dfault)) dfault = " Default " + dfault;

                    var notNull = attr.SelectSingleNode("NOTNULL").InnerText == "1" ? " NOT NULL" : string.Empty;

                    var def = attr.SelectSingleNode("DEF").InnerText;
                    if (!string.IsNullOrWhiteSpace(def)) def = " " + def; // TODO remove when refactoring

                    var uniqueConstraint = string.Empty;
                    var isUnique = attr.SelectSingleNode("UNIQUE").InnerText == "1" ? " UNIQUE" : string.Empty;

                    if (!string.IsNullOrWhiteSpace(isUnique))
                    {
                        uniqueConstraint = getConstraint(true, attr.SelectSingleNode("UNIQUECONSTRAINT").InnerText, tableName, columnName);
                        if (!string.IsNullOrWhiteSpace(uniqueConstraint))
                            uniqueConstraint = " Constraint " + uniqueConstraint;
                    }

                    var checkConstraint = getConstraint(true, attr.SelectSingleNode("CHECKCONSTRAINT").InnerText, tableName, columnName);
                    if (!string.IsNullOrWhiteSpace(checkConstraint)) checkConstraint = " Constraint " + checkConstraint;

                    var check = string.Empty;
                    var checkPattern = attr.SelectSingleNode("CHECK").InnerText;
                    if (checkPattern != typeCheck)
                    {
                        check = getConstraint(false, checkPattern, tableName, columnName);
                        if (!string.IsNullOrWhiteSpace(check)) check = $" Check ({check})";
                    }

                    if (!string.IsNullOrWhiteSpace(uniqueConstraint) && !string.IsNullOrWhiteSpace(checkConstraint))
                        throw new InvalidOperationException("both constraints present please validate output");

                    sb.AppendLine($"\t{columnName} {dataType}{dfault}{def}{notNull}{checkConstraint}{check}{uniqueConstraint}{isUnique},");

                    var isPk = attr.SelectSingleNode("PK").InnerText == "1";
                    if (isPk)
                        primaryKeys.Add(columnName);
                }

                if (primaryKeys.Count > 0)
                    sb.AppendLine($"{pkConstraint} Primary Key ({string.Join(",", primaryKeys.ToArray())})");
                //sb.AppendLine($"\t{pkConstraint}PRIMARY KEY ({string.Join(",", primaryKeys.ToArray())})");

                sb.AppendLine(");");

                sb.AppendLine();
            }

            #endregion entities

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();

            #region indexes

            foreach (XmlNode node in entities)
            {
                var tableName = node.SelectSingleNode("NAME").InnerText;

                foreach (XmlNode idx in node.SelectSingleNode("INDEXES").ChildNodes)
                {
                    var name = idx.SelectSingleNode("NAME").InnerText;
                    var unique = idx.SelectSingleNode("UNIQUE").InnerText == "1" ? " UNIQUE" : string.Empty;
                    var descending = idx.SelectSingleNode("DESCENDING").InnerText == "1" ? " DESCENDING" : string.Empty;

                    var columns = new List<string>();
                    foreach (XmlNode item in idx.SelectNodes("INDEXITEM"))
                        columns.Add(item.SelectSingleNode("NAME").InnerText);

                    if (columns.Count == 0)
                        throw new InvalidOperationException("empty index on table: " + tableName);

                    sb.AppendLine($"Create{unique}{descending} Index {name}  ON {tableName} ({string.Join(",", columns.ToArray())});");
                }
            }

            #endregion indexes

            XmlNode GetEntity(string id)
            {
                foreach (XmlNode entity in entities)
                    if (entity.SelectSingleNode("ID").InnerText == id)
                        return entity;

                return null;
            }

            foreach (XmlNode node in entities)
            {
                foreach (XmlNode relace in node.SelectSingleNode("RELATIONS").ChildNodes)
                {
                    var fkConstraint = relace.SelectSingleNode("FKCONSTRAINT").InnerText;
                    var idParent = relace.SelectSingleNode("PARENTENTITYID").InnerText;
                    var idChild = relace.SelectSingleNode("CHILDENTITYID").InnerText;

                    var parent = GetEntity(idParent);
                    var child = GetEntity(idChild);

                    // TODO ...
                    sb.Append($"Alter Table {child.SelectSingleNode("NAME").InnerText} add Constraint {fkConstraint} Foreign Key ");
                }
            }

            return sb.ToString();
        }
    }
}