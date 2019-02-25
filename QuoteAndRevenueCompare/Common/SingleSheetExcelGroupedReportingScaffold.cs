using NPOI.SS.UserModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public class GroupKey
    {
        public string Name { get; set; }
        public Type Type { get; set; }
    }

    public class SingleSheetExcelGroupedReportingScaffold<TFather, TChild> : SingelSheetExcelScaffold<TFather>, IExceptType
        where TFather : class
        where TChild : class
    {
        private List<TFather> _fatherSource;
        private Type fatherType;
        private Type childType;
        private PropertyInfo[] fatherPropertys;
        private PropertyInfo[] childPropertys;

        private List<TChild> _childSource;
        private string _groupField;
        private Type _groupFieldType;
        private Dictionary<string, int> childHeadNames;
        private int curStratGroupRowIndex;
        private int curEndGroupRowIndex;

        private SingleSheetExcelGroupedReportingScaffold()
            : base()
        {
        }

        public override void SetExceptTypes(List<Type> types)
        {
            throw new NotImplementedException();
        }

        public SingleSheetExcelGroupedReportingScaffold(List<TFather> fathers, List<TChild> childs, string groupField)
            : this()
        {
            if (fathers == null || childs == null)
                throw new ArgumentNullException();
            _fatherSource = fathers;
            _childSource = childs;
            _groupField = groupField;
            fatherType = typeof(TFather);
            childType = typeof(TChild);
            fatherPropertys = fatherType.GetProperties();
            childPropertys = childType.GetProperties();
            childHeadNames = new Dictionary<string, int>();
            int currentColumnInex = 0;
            foreach (var prop in childPropertys)
            {
                if (prop.Name == _groupField)
                    continue;
                if (ExcelHeadNotContainedTypeFactory.GetExceptedTypes().Contains(prop.PropertyType))
                    continue;
                childHeadNames.Add(prop.Name, currentColumnInex);
                currentColumnInex++;
            }
            Check();
            GenerateExecelContent();
        }

        private void Check()
        {
            PropertyInfo prop1 = fatherType.GetProperty(_groupField);
            PropertyInfo prop2 = childType.GetProperty(_groupField);
            if (prop1 == null || prop2 == null)
                throw new Exception("Group field is not valid");

            if (prop1.PropertyType.Name != prop2.PropertyType.Name)
                throw new Exception("Group field type is not same in father type and child type");
            _groupFieldType = prop1.PropertyType;
        }

        private void GenerateExecelContent()
        {
            foreach (var groupItem in _fatherSource)
            {
                SetFatherRowContent(groupItem);
                object value = typeof(TFather).GetProperty(_groupField).GetValue(groupItem);
                SetChildrenRowContent(value);
                curEndGroupRowIndex = currentRowIndex;
                GroupRow(curStratGroupRowIndex, curEndGroupRowIndex);
            }
        }

        private void SetChildrenRowContent(object fatherRefObject)
        {
            List<TChild> groupedChilds = _childSource.Where(ExpressionHelper.GetMeberEqualValueLambda<TChild>(_groupField, _groupFieldType, fatherRefObject).Compile()).ToList();
            if (groupedChilds.Count == 0)
                return;
            foreach (var childColumnName in childHeadNames)
            {
                SetCellContent(currentRowIndex, childColumnName.Value, childColumnName.Key);
            }
            currentRowIndex++;

            foreach (var childItem in groupedChilds)
            {
                foreach (var keyvalue in childHeadNames)
                {
                    object obj = typeof(TChild).GetProperty(keyvalue.Key).GetValue(childItem);
                    if (obj == null)
                        continue;
                    string content = obj.ToString();
                    SetCellContentByColumnName(currentRowIndex, keyvalue.Key, content);
                }
                currentRowIndex++;
            }
        }

        private void SetFatherRowContent(TFather fatherItem)
        {
            curStratGroupRowIndex = currentRowIndex + 1;
            foreach (var item in HeadNames)
            {
                object proValue = fatherType.GetProperty(item.Key).GetValue(fatherItem);
                if (proValue == null)
                    continue;
                base.SetCellContentByColumnName(currentRowIndex, item.Key, proValue.ToString());
            }
            currentRowIndex++;
        }

        private void GroupRow(int startGroupRowIndex, int endGroupRowIndex)
        {
            if (startGroupRowIndex <= 0 || endGroupRowIndex <= 0)
                throw new Exception("startGroupRowIndex or endGroupRowIndex is not positive");
            if (startGroupRowIndex >= endGroupRowIndex)
                throw new Exception("startGroupRowIndex can not >= endGroupRowIndex");
            if (startGroupRowIndex == endGroupRowIndex)
                return;

            sheet.GroupRow(startGroupRowIndex, endGroupRowIndex);
            sheet.SetRowGroupCollapsed(startGroupRowIndex, true);
        }

        public override void SetCellContentByColumnName(int rowIndex, string columnName, string value)
        {
            int curcolumnIndex;
            if (childHeadNames.TryGetValue(columnName, out curcolumnIndex))
            {
                SetCellContent(rowIndex, curcolumnIndex, value);
            }
        }
    }

    public class SingleSheetExcelGroupedReportingScaffold<TFather, TChild1, TChild2> : SingelSheetExcelScaffold<TFather>, IExceptType
        where TFather : class
        where TChild1 : class
        where TChild2 : class
    {
        private List<TFather> _fatherSource;
        private List<TChild1> _child1Source;
        private List<TChild2> _child2Source;
        private Type fatherType;
        private Type child1Type;
        private Type child2Type;
        private PropertyInfo[] fatherPropertys;
        private PropertyInfo[] child1Propertys;
        private PropertyInfo[] child2Propertys;

        private string _groupField;
        private Type _groupFieldType;
        private Dictionary<string, int> child1HeadNames;
        private Dictionary<string, int> child2HeadNames;
        private int curStratGroupRowIndex;
        private int curEndGroupRowIndex;

        private SingleSheetExcelGroupedReportingScaffold()
            : base()
        {
        }

        public override void SetExceptTypes(List<Type> types)
        {
            throw new NotImplementedException();
        }

        public SingleSheetExcelGroupedReportingScaffold(List<TFather> fathers, List<TChild1> childs1, List<TChild2> childs2, string groupField)
            : this()
        {
            if (fathers == null || childs1 == null || childs2 == null)
                throw new ArgumentNullException();
            _fatherSource = fathers;
            _child1Source = childs1;
            _child2Source = childs2;
            _groupField = groupField;
            fatherType = typeof(TFather);
            child1Type = typeof(TChild1);
            child2Type = typeof(TChild2);
            fatherPropertys = fatherType.GetProperties();

            child1Propertys = child1Type.GetProperties();
            child2Propertys = child2Type.GetProperties();
            child1HeadNames = new Dictionary<string, int>();
            child2HeadNames = new Dictionary<string, int>();
            int currentColumnInex = 0;
            foreach (var prop in child1Propertys)
            {
                if (prop.Name == _groupField)
                    continue;
                if (ExcelHeadNotContainedTypeFactory.GetExceptedTypes().Contains(prop.PropertyType))
                    continue;
                child1HeadNames.Add(prop.Name, currentColumnInex);
                currentColumnInex++;
            }

            currentColumnInex += 2;

            foreach (var prop in child2Propertys)
            {
                if (prop.Name == _groupField)
                    continue;
                if (ExcelHeadNotContainedTypeFactory.GetExceptedTypes().Contains(prop.PropertyType))
                    continue;
                child2HeadNames.Add(prop.Name, currentColumnInex);
                currentColumnInex++;
            }
            Check();
            GenerateExecelContent();
        }

        private void Check()
        {
            PropertyInfo prop1 = fatherType.GetProperty(_groupField);
            PropertyInfo prop2 = child1Type.GetProperty(_groupField);
            PropertyInfo prop3 = child2Type.GetProperty(_groupField);
            if (prop1 == null || prop2 == null || prop3 == null)
                throw new Exception("Group field is not valid");

            if (prop1.PropertyType.Name == prop2.PropertyType.Name && prop1.PropertyType.Name == prop3.PropertyType.Name)
            { }
            else
                throw new Exception("Group field type is not same in father type and child type");
            _groupFieldType = prop1.PropertyType;
        }

        private void GenerateExecelContent()
        {
            foreach (var groupItem in _fatherSource)
            {
                SetFatherRowContent(groupItem);
                object value = typeof(TFather).GetProperty(_groupField).GetValue(groupItem);
                SetChildrenRowContent(value);
                curEndGroupRowIndex = currentRowIndex;
                GroupRow(curStratGroupRowIndex, curEndGroupRowIndex);
            }
        }

        private void SetChildrenRowContent(object fatherRefObject)
        {
            List<TChild1> groupedChilds1 = _child1Source.Where(ExpressionHelper.GetMeberEqualValueLambda<TChild1>(_groupField, _groupFieldType, fatherRefObject).Compile()).ToList();
            List<TChild2> groupedChilds2 = _child2Source.Where(ExpressionHelper.GetMeberEqualValueLambda<TChild2>(_groupField, _groupFieldType, fatherRefObject).Compile()).ToList();

            if (groupedChilds1.Count == 0 && groupedChilds2.Count == 0)
                return;

            foreach (var childColumnName in child1HeadNames)
            {
                SetCellContent(currentRowIndex, childColumnName.Value, childColumnName.Key);
            }
            foreach (var childColumnName in child2HeadNames)
            {
                SetCellContent(currentRowIndex, childColumnName.Value, childColumnName.Key);
            }

            currentRowIndex++;
            ConcurrentQueue<int> curRowIndexLogs = new ConcurrentQueue<int>();
            foreach (var childItem in groupedChilds1)
            {
                foreach (var keyvalue in child1HeadNames)
                {
                    object obj = typeof(TChild1).GetProperty(keyvalue.Key).GetValue(childItem);
                    if (obj == null)
                        continue;
                    string content = obj.ToString();
                    SetCellContentByColumnName(currentRowIndex, keyvalue.Key, content);
                }
                curRowIndexLogs.Enqueue(currentRowIndex);
                currentRowIndex++;
            }

            foreach (var childItem in groupedChilds2)
            {
                int beforeRowIndex;
                if (curRowIndexLogs.TryDequeue(out beforeRowIndex))
                {
                    foreach (var keyvalue in child2HeadNames)
                    {
                        object obj = typeof(TChild2).GetProperty(keyvalue.Key).GetValue(childItem);
                        if (obj == null)
                            continue;

                        string content = obj.ToString();
                        SetCellContentByColumnName(beforeRowIndex, keyvalue.Key, content);
                    }
                }
                else
                {
                    foreach (var keyvalue in child2HeadNames)
                    {
                        object obj = typeof(TChild2).GetProperty(keyvalue.Key).GetValue(childItem);
                        if (obj == null)
                            continue;

                        string content = obj.ToString();
                        SetCellContentByColumnName(currentRowIndex, keyvalue.Key, content);
                    }
                    currentRowIndex++;
                }
            }
        }

        private void SetFatherRowContent(TFather fatherItem)
        {
            curStratGroupRowIndex = currentRowIndex + 1;
            foreach (var item in HeadNames)
            {
                object proValue = fatherType.GetProperty(item.Key).GetValue(fatherItem);
                if (proValue == null)
                    continue;
                base.SetCellContentByColumnName(currentRowIndex, item.Key, proValue.ToString());
            }
            currentRowIndex++;
        }

        private void GroupRow(int startGroupRowIndex, int endGroupRowIndex)
        {
            if (startGroupRowIndex <= 0 || endGroupRowIndex <= 0)
                throw new Exception("startGroupRowIndex or endGroupRowIndex is not positive");
            if (startGroupRowIndex >= endGroupRowIndex)
                throw new Exception("startGroupRowIndex can not >= endGroupRowIndex");
            if (startGroupRowIndex == endGroupRowIndex)
                return;

            sheet.GroupRow(startGroupRowIndex, endGroupRowIndex);
            sheet.SetRowGroupCollapsed(startGroupRowIndex, true);
        }

        public override void SetCellContentByColumnName(int rowIndex, string columnName, string value)
        {
            int curcolumnIndex;
            if (child1HeadNames.TryGetValue(columnName, out curcolumnIndex))
            {
                SetCellContent(rowIndex, curcolumnIndex, value);
            }
            if (child2HeadNames.TryGetValue(columnName, out curcolumnIndex))
            {
                SetCellContent(rowIndex, curcolumnIndex, value);
            }
        }

        public void SetTotal<TTotal>(TTotal value)
        {
            currentRowIndex += 4;
            Type type = typeof(TTotal);
            int columnIndex = 0;
            PropertyInfo[] props = type.GetProperties();
            foreach (var pro in props)
            {
                SetCellContent(currentRowIndex, columnIndex, pro.Name);
                object obj = pro.GetValue(value);
                if (obj == null)
                { }
                else
                {
                    SetCellContent(currentRowIndex + 1, columnIndex, obj.ToString());
                }
                columnIndex++;
            }

            currentRowIndex += 2;
        }
    }
}