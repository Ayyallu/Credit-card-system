/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : Currency.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Currency model
      Author				:	Amal    
      Date					:	03.03.2014
      Function				:	model for Currency master
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("Mt_Currency")]
    public class Currency
    {

        #region Model

        [Key , Column (Order = 1)]       
        public string CurrencyCode { get; set; }
        [Key, Column(Order = 0)]
        public string OrganizationCode { get; set; }
        public string CurrencyName { get; set; }
        public string CountryCode { get; set; }
        public decimal Precision{ get; set; }
        public decimal? UFCLPRate { get; set; }
        public bool IsBlackListed { get; set; }
        public bool IsActive { get; set; }

        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        


        
        
        #endregion
    }
}
