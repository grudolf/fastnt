﻿using FasTnT.Domain;
using FasTnT.Model.Exceptions;
using FasTnT.Model.Queries.PredefinedQueries.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FasTnT.Formatters.Xml.Requests.Queries
{
    public class SimpleEventQueryParameterFactory
    {
        internal static IEnumerable<SimpleEventQueryParameter> ParseParameters(IEnumerable<XElement> elements)
        {
            foreach(var element in elements ?? new XElement[0])
            {
                var name = element.Element("name")?.Value?.Trim();
                var values = element.Element("value")?.HasElements ?? false ? element.Element("value").Elements().Select(x => x.Value) : new[] { element.Element("value")?.Value };

                if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Query parameter element does not contain name element");
                else if (Equals(name, "eventType")) yield return new EventTypeParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^(GE|LT)_eventTime$")) yield return new EventTimeParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^(GE|LT)_recordTime$")) yield return new RecordTimeParameter { Name = name, Values = values };
                else if (Equals(name, "EQ_eventID")) yield return new EventIdParameter { Name = name, Values = values };
                else if (Equals(name, "EQ_action")) yield return new ActionParameter { Name = name, Values = values };
                else if (Equals(name, "EQ_bizStep")) yield return new BizStepParameter { Name = name, Values = values };
                else if (Equals(name, "EQ_disposition")) yield return new DispositionParameter { Name = name, Values = values };
                else if (Equals(name, "EQ_readPoint")) yield return new ReadPointParameter { Name = name, Values = values };
                else if (Equals(name, "WD_readPoint")) throw new NotImplementedException($"Parameter '{name}' is not implemented yet");
                else if (Equals(name, "EQ_bizLocation")) yield return new BizLocationParameter { Name = name, Values = values };
                else if (Equals(name, "WD_bizLocation")) throw new NotImplementedException($"Parameter '{name}' is not implemented yet");
                else if (Regex.IsMatch(name, "^EQ_bizTransaction_")) yield return new BizTransactionParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^EQ_source_")) yield return new SourceDestinationParameter { Name = name, Values = values, Direction = SourceDestinationType.Source };
                else if (Regex.IsMatch(name, "^EQ_destination_")) yield return new SourceDestinationParameter { Name = name, Values = values, Direction = SourceDestinationType.Destination };
                else if (Equals(name, "EQ_transformationID")) yield return new TransformationParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^MATCH_anyEPC")) yield return new MatchAnyEpcParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^MATCH_")) yield return new MatchEpcParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^(EQ|GT|GE|LT|LE)_quantity$")) yield return new QuantityParameter { Name = name, Values = values };
                else if (Equals(name, "EXISTS_errorDeclaration")) yield return new ErrorDeclarationParameter { Name = name, Values = values };
                else if (Equals(name, "orderBy")) yield return new OrderByParameter { Name = name, Values = values };
                else if (Equals(name, "orderDirection")) yield return new OrderDirectionParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^(GE|LT)_errorDeclarationTime$")) throw new EpcisException(ExceptionType.ImplementationException, $"Parameter '{name}' is not implemented yet");
                else if (Equals(name, "EQ_errorReason")) throw new EpcisException(ExceptionType.ImplementationException, $"Parameter '{name}' is not implemented yet");
                else if (Equals(name, "EQ_correctiveEventID")) throw new EpcisException(ExceptionType.ImplementationException, $"Parameter '{name}' is not implemented yet");
                else if (Equals(name, "eventCountLimit")) yield return new EventCountLimitParameter { Name = name, Values = values };
                else if (Equals(name, "maxEventCount")) yield return new MaxEventCountParameter { Name = name, Values = values };
                else if (Regex.IsMatch(name, "^(EQ|GT|GE|LT|LE)_INNER_ILMD_")) yield return new CustomFieldParameter { Name = name, IsInner = true, Values = values, FieldType = FieldType.Ilmd };
                else if (Regex.IsMatch(name, "^(EQ|GT|GE|LT|LE)_ILMD_")) yield return new CustomFieldParameter { Name = name, IsInner = false, Values = values, FieldType = FieldType.Ilmd };
                else if (Regex.IsMatch(name, "^EXISTS_INNER_ILMD_")) yield return new ExistsCustomFieldParameter { Name = name, IsInner = true, Values = values, FieldType = FieldType.Ilmd };
                else if (Regex.IsMatch(name, "^EXISTS_ILMD_")) yield return new ExistsCustomFieldParameter { Name = name, IsInner = false, Values = values, FieldType = FieldType.Ilmd };
                else if (Regex.IsMatch(name, "^(EQ|GT|GE|LT|LE)_INNER_")) yield return new CustomFieldParameter { Name = name, IsInner = true, Values = values, FieldType = FieldType.EventExtension };
                else if (Regex.IsMatch(name, "^(EQ|GT|GE|LT|LE)_")) yield return new CustomFieldParameter { Name = name, IsInner = false, Values = values, FieldType = FieldType.EventExtension };
                else if (Regex.IsMatch(name, "^EXISTS_INNER_")) yield return new ExistsCustomFieldParameter { Name = name, IsInner = true, Values = values, FieldType = FieldType.EventExtension };
                else if (Regex.IsMatch(name, "^EXISTS_")) yield return new ExistsCustomFieldParameter { Name = name, IsInner = false, Values = values, FieldType = FieldType.EventExtension };
                // TODO: missing ATTR parameters (LAA)

                else throw new EpcisException(ExceptionType.QueryParameterException, $"Query parameter with name '{name}' not expected.");
            }
        }
    }
}
