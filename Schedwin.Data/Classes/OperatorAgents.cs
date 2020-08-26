using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Schedwin.Data.Classes
{
    public class OperatorAgent
    {
        public int IDX { get; set; }
        public String Description { get; set; }

        public int IDX_Operator { get; set; }

     
        private static List<OperatorAgent> _listOperatorAgents { get; set; }


        public static explicit operator OperatorAgent (vl_CompanyOperatorsAgents vlCompanyOperatorAgent)
        {
            var operatorAgent = new OperatorAgent();
            operatorAgent.IDX = vlCompanyOperatorAgent.IDX;
            operatorAgent.Description = vlCompanyOperatorAgent.Agent;
            operatorAgent.IDX_Operator = vlCompanyOperatorAgent.IDX_Operator;
            return operatorAgent;
        }

        public static explicit operator OperatorAgent (tbOperatorAgent tbOperatorAgent)
        {
            var operatorAgent = new OperatorAgent();
            operatorAgent.IDX = tbOperatorAgent.pkOperatorAgentID;
            operatorAgent.Description = tbOperatorAgent.Description;
            operatorAgent.IDX_Operator = tbOperatorAgent.fkCompanyID;
            return operatorAgent;
        }

        public static async Task< List<OperatorAgent>> LoadOperatorAgents()
        {
            if (_listOperatorAgents == null)
            {

                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                   var tmpOperatorAgents = await ctx.tbOperatorAgents.Where(x => x.Active == true).ToListAsync();
                    _listOperatorAgents = tmpOperatorAgents.Select(x => (OperatorAgent)x).ToList();

                }
            }

            return _listOperatorAgents;
        }

        static public async Task<List<OperatorAgent>> GetOperatorAgents(String Server, String dbInstance)
        {

            if (_listOperatorAgents == null)
            {
                var conString = RegionalConnectionGenerator.GetConnectionString(Server, dbInstance);
                var ctx = new SchedwinRegionalEntities(conString);


                using (ctx)
                {
                    var tmpCompanyOperatorAgents = await ctx.vl_CompanyOperatorsAgents.Where(x => x.Active == true).ToListAsync();
                    _listOperatorAgents = tmpCompanyOperatorAgents.Select(x => (OperatorAgent)x).ToList();


                }
            }

            return _listOperatorAgents;

        }

      static public OperatorAgent GetOperatorAgentDetails(int AgentIDX)
        {
            if (_listOperatorAgents!=null)
            {
                return _listOperatorAgents.FirstOrDefault(x => x.IDX == AgentIDX);
            };
            return null;
        }

        static public async Task<List<OperatorAgent>> GetAgentsForOperator(int OperatorIDX)
        {
            var tmpLst = await LoadOperatorAgents();
            return tmpLst.Where(x => x.IDX_Operator == OperatorIDX).ToList();
        }

        static public async Task<List<OperatorAgent>> GetAgentsForOperator(int OperatorIDX,String Server, String dbInstance)
        {
            var tmpLst=await GetOperatorAgents(Server, dbInstance);
            return tmpLst.Where(x => x.IDX_Operator == OperatorIDX).ToList();
        }
    }
}
