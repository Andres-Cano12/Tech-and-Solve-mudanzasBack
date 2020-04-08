using AccessData.Context;
using AccessData.Entities;
using App.Common.Classes.Base.Repositories;
using Common.Classes.BussinesLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Repository
{
    public class MoveRepository : BaseCRUDRepository<Move>, IMoveRepository
    {
        public DBContext Context
        {
            get
            {
                return (DBContext)_Database;
            }
        }

        public MoveRepository(DBContext database)
            : base(database)
        {

        }
        public async Task<List<MoveDetail>> CreateMove(FileDTO file)
        {
                var listMoveDetail = new List<MoveDetail>();
                Move move = new Move
                {
                    IdMove = 0,
                    DateMove = DateTime.Now,
                    IdentificationCard = file.Id
                };

                using (var reader = new StreamReader(file.File.OpenReadStream()))
                {
                    int index = 1;
                    while (reader.Peek() >= 0)
                    {
                        var moveDetail =
                            new MoveDetail
                            {
                                IdMove = move.IdMove,
                                Value = Int32.Parse(reader.ReadLine()),
                                Position = index
                            };
                        listMoveDetail.Add(moveDetail);
                        index++;
                    }
                }

               move.MoveDetail = listMoveDetail;

               await Context.AddAsync(move);
               Context.SaveChanges();
               return listMoveDetail;
        }
    }
}
