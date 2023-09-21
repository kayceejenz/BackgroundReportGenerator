using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackgroundReportGenerator
{
    public class DataAccessor
    {
        private readonly DbContext dbContext;
        private static DataAccessor instance;

        private DataAccessor(DbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public static DataAccessor GetInstance(DbContext dbContext)
        {
            if (instance == null)
            {
                instance = new DataAccessor(dbContext);
            }
            return instance;
        }

        public List<T> Fetch<T>(Func<T, bool> predicate, ref string cursor, uint limit = 10) where T : class
        {
            var query = dbContext.Set<T>().AsQueryable();

            if (cursor != null && int.TryParse(cursor, out int cursorValue))
            {
                query = query.Where(item => GetCursorValue(item) > cursorValue);
            }

            var records = query.Where(predicate).Take((int)limit).ToList();

            if (records.Any())
                cursor = GetCursorValue(records.Last()).ToString();


            return records;
        }

        public T Create<T>(T data) where T : class
        {
            dbContext.Set<T>().Add(data);
            dbContext.SaveChanges();

            return data;
        }

        public void Update<T>(Func<T, bool> predicate, T data) where T : class
        {
            var existingRecord = dbContext.Set<T>().FirstOrDefault(predicate);

            if (existingRecord != null)
            {
                dbContext.Entry(existingRecord).CurrentValues.SetValues(data);
                dbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Record not found for update.");
            }
        }


        private static int GetCursorValue<T>(T entity)
        {
            PropertyInfo property = typeof(T).GetProperty("Id");

            if (property != null)
            {
                object value = property.GetValue(entity);

                if (value is int id)
                    return id;
            }

            throw new InvalidOperationException("Unable to determine the cursor value.");
        }
    }
}