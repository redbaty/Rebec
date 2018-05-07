using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Rebec.Interfaces;
using Rebec.Representations;

namespace Rebec.Builders
{
    public class TableBuilder : ITableBuilderContext, IBuilder
    {
        public IElement Build()
        {
            return CreateTable(Style);
        }

        public ICollection<ColumnRepresentation> Columns { get; } = new List<ColumnRepresentation>();

        public ICollection Objects { get; private set; }

        public IBuilderStyle Style { get; private set; }

        private IList<string> AdditionalRows { get; } = new List<string>();

        public TableBuilder WithAdditionalRow(string row)
        {
            AdditionalRows.Add(row);
            return this;
        }

        public TableBuilder WithItems<T>(IEnumerable<T> enumerable)
        {
            Objects = enumerable.ToList();
            return this;
        }

        public TableBuilder WithColumn(string column, PropertyInfo propertyInfo)
        {
            Columns.Add(new ColumnRepresentation(column, propertyInfo));
            return this;
        }

        public TableBuilder WithComputedColumn(string column, Func<object, string> action)
        {
            Columns.Add(new ColumnRepresentation(column, action));
            return this;
        }

        public TableBuilder WithComputedColumn<T>(string column, Func<T, string> action)
        {
            Columns.Add(new ColumnRepresentation(column, obj =>
            {
                if (obj is T t) return action.Invoke(t);

                return string.Empty;
            }));
            return this;
        }

        public TableBuilder WithColumns(PropertyInfo propertyInfo, params string[] columns)
        {
            foreach (var column in columns)
                Columns.Add(new ColumnRepresentation(column, propertyInfo));

            return this;
        }

        public TableBuilder UseItemsPropertiesAsColumns()
        {
            var type = Objects.GetType().GetGenericArguments().FirstOrDefault();

            if (type == null) throw new InvalidOperationException("Could not find the IEnumerable object type");

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                WithColumn(propertyInfo.Name, propertyInfo);

            return this;
        }

        public TableBuilder WithStyling(IBuilderStyle styling)
        {
            Style = styling;
            return this;
        }

        private IHtmlTableElement CreateTable(IBuilderStyle styling = null)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(string.Empty);

            var tableElement = document.CreateElement<IHtmlTableElement>();
            CreateColumns(tableElement);
            WriteValues(tableElement);

            if (!string.IsNullOrEmpty(styling?.Class))
                tableElement.SetAttribute("class", styling.Class);

            return tableElement;
        }

        private void WriteValues(IHtmlTableElement tableElement)
        {
            var htmlTableSectionElement = tableElement.CreateBody();

            foreach (var obj in Objects)
            {
                var row = htmlTableSectionElement.InsertRowAt();

                foreach (var column in Columns)
                    row.InsertCellAt().InnerHtml = column.IsComputed && column.ComputedAction != null
                        ? column.ComputedAction.Invoke(obj)
                        : column.RefersTo.GetValue(obj).ToString();
            }

            foreach (var additionalRow in AdditionalRows)
            {
                var parser = new HtmlParser();
                var row = parser.ParseFragment(additionalRow, tableElement).FirstOrDefault()?.FirstChild;
                htmlTableSectionElement.AppendChild(row);
            }
        }

        private void CreateColumns(IHtmlTableElement tableElement)
        {
            var htmlTableRowElement = tableElement.CreateHead().InsertRowAt();

            foreach (var column in Columns) htmlTableRowElement.InsertCellAt().InnerHtml = column.ToString();
        }
    }
}