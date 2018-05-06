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

namespace Rebec.Builders
{
    public class TableBuilder : ITableBuilderContext, IBuilder
    {
        public IElement Build()
        {
            return CreateTable(Style);
        }

        public ICollection Objects { get; private set; }
        public ICollection<string> Columns { get; } = new List<string>();
        public ICollection<PropertyInfo> Properties { get; } = new List<PropertyInfo>();
        public IBuilderStyle Style { get; private set; }

        public TableBuilder WithItems<T>(IEnumerable<T> enumerable)
        {
            Objects = enumerable.ToList();
            return this;
        }

        public TableBuilder WithColumn(string column)
        {
            Columns.Add(column);
            return this;
        }

        public TableBuilder WithColumns(params string[] columns)
        {
            foreach (var column in columns)
                Columns.Add(column);

            return this;
        }

        public TableBuilder UseItemsPropertiesAsColumns()
        {
            var type = Objects.GetType().GetGenericArguments().FirstOrDefault();

            if (type == null) throw new InvalidOperationException("Could not find the IEnumerable object type");

            Properties.Clear();

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Properties.Add(propertyInfo);
                WithColumn(propertyInfo.Name);
            }

            return this;
        }

        public TableBuilder WithColumns(IEnumerable<string> columns)
        {
            foreach (var column in columns)
                Columns.Add(column);

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

            foreach (var o in Objects)
            {
                var row = htmlTableSectionElement.InsertRowAt();

                foreach (var propertyInfo in Properties)
                    row.InsertCellAt().InnerHtml = propertyInfo.GetValue(o).ToString();
            }
        }

        private void CreateColumns(IHtmlTableElement tableElement)
        {
            var htmlTableRowElement = tableElement.CreateHead().InsertRowAt(0);

            foreach (var column in Columns) htmlTableRowElement.InsertCellAt().InnerHtml = column;
        }
    }
}