using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
using Microsoft.AspNetCore.Http;

namespace QueryFilterServiceApp.Services {
    public class SelectQueryFilterService : ISelectQueryFilterService {
        readonly int studentId;

        public SelectQueryFilterService(IUserService userService, IHttpContextAccessor contextAccessor) {
		// Beta version workaround: 
		// an instance of the SelectQueryFilterService class is created (but not used) 
		// at program startup when http context is not available.
            if(contextAccessor.HttpContext != null) {   
                studentId = userService.GetCurrentUserId();
            }
        }

        public CriteriaOperator CustomizeFilterExpression(
            SelectQuery query, CriteriaOperator filterExpression) 
        {
            List<CriteriaOperator> filters = new List<CriteriaOperator>();

            if(query.Tables.Any(x => x.ActualName == "Students")) {
                filters.Add(new BinaryOperator
                    (new OperandProperty("Students.ID"), 
                    new OperandValue(studentId), 
                    BinaryOperatorType.Equal));
            }

            if(query.Tables.Any(x => x.ActualName == "Enrollments")) {
                filters.Add(new BinaryOperator
                    (new OperandProperty("Enrollments.StudentID"), 
                    new OperandValue(studentId), 
                    BinaryOperatorType.Equal));
            }

            if(query.Tables.Any(x => x.ActualName == "Reports")) {
                filters.Add(new BinaryOperator
                    (new OperandProperty("Reports.StudentID"), 
                    new OperandValue(studentId), 
                    BinaryOperatorType.Equal));
            }

            if(!object.ReferenceEquals(filterExpression, null)) {
                filters.Add(filterExpression);
            }

            var result = filters.Any() ? new GroupOperator(GroupOperatorType.And, filters) : null;
            return result;
        }
    }
}
