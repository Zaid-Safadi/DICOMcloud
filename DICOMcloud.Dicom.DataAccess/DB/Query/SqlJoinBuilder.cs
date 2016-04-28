using DICOMcloud.Dicom.DataAccess.DB.Schema;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.DB.Query
{
    class SqlJoinBuilder
    {
        private static ConcurrentDictionary<DbJoin, ICollection<string>> _cachedJoins = new ConcurrentDictionary<DbJoin,ICollection<string>> ( ) ;
        private Dictionary<int,string> _generatedJoins = new Dictionary<int,string> ( ) ;

        public void AddJoins ( TableKey source, TableKey destination )
        { 
            if ( source == destination ) return ;

            DbJoin join = new DbJoin ( source, destination ) ;

            CreateJoin ( join ) ;
        }

        public override string ToString()
        {
            return string.Join ( " ", _generatedJoins.Values ) ;
        }

        private void CreateJoin ( DbJoin arg )
        {
            TableKey childTable = arg.Source ;
            TableKey parent = null ;

            while ( (parent = childTable.Parent ) != null && ( arg.Destination != childTable))
            { 
                DbJoin join = new DbJoin ( childTable, parent ) ;

                if ( !_generatedJoins.ContainsKey ( join ) )
                { 
                    var joins = _cachedJoins.GetOrAdd ( join, GenerateAndAddNewJoin ) ;
                    _generatedJoins.Add ( join, joins.First ( ) ) ;
                }

                childTable = parent ;
            }
        }

        private List<string> GenerateAndAddNewJoin(DbJoin arg)
        {
            string joinText = GetJoinWithParent ( arg.Source ) ;
        
            return new List<string> ( new string[]{joinText} ) ;
        }

        private string GetJoinWithParent(TableKey table )
        {
            //{0}=Patient (parent/destination)
            //{1}=Study (child/source)
            //{2}=Study_PatientKey (child foriegn)
            //{3}=PatientKey (parent foriegn)

            return string.Format ( SqlQueries.Joins.JoinFormattedTemplate,
                                                table.Parent.Name, 
                                                table.Name, 
                                                table.ForeignColumn.Name, 
                                                table.Parent.KeyColumn.Name ) ;

        }
    }
    
    class DbJoin : IComparable<DbJoin>
    { 
        public DbJoin ( TableKey source, TableKey destination )
        { 
            Source      = source ;
            Destination = destination ;

            _key = GenerateKey ( source, destination ) ;
        }

        public static int GenerateKey(TableKey source, TableKey destination )
        {
            return (int) (source.OrderValue << 16 | (destination.OrderValue) );
        }

        int _key ;
        public TableKey Source      {get; set;}
        public TableKey Destination {get; set;}
    
        public static implicit operator int(DbJoin join )
        {
            return join._key ;
        }

        public override int GetHashCode()
        {
            return _key ;
        }

        public int CompareTo(DbJoin obj)
        {
            return _key.CompareTo ( obj._key ) ;
        }

        public override bool Equals(object obj)
        {
            if (null == obj) {  return false ;}

            if (!(obj is DbJoin)) {  return false ; }

            return ((DbJoin) obj)._key == _key ;
        }
    }
}
