using AccessData.Entities;
using AccessData.Repository;
using AccessData.Repository.IMoveDetailRepository;
using App.Common.Classes.Base.Repositories;
using App.Common.Classes.Base.Services;
using App.Common.Classes.Constants;
using App.Common.Classes.DTO.Common;
using App.Common.Classes.Helpers;
using App.Common.Classes.Validator;
using App.Common.Resources;
using App.Common.Resources.Distributors;
using AutoMapper;
using AutoMapper.Configuration;
using Common.Classes.BussinesLogic;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinnesLogic.Services
{
    public class MoveService : BaseService<MoveDTO, Move>, IMoveService
    {
        private IMoveRepository moveRepository;
        private IMoveDetailRepository moveDetailRepository;
        private IMapper _mapper;

        public MoveService(IBaseCRUDRepository<Move> repository, App.Common.Classes.Cache.IMemoryCacheManager
            memoryCacheManager, IMapper mapper, IServiceValidator<Move> validation
            , Microsoft.Extensions.Configuration.IConfiguration configuration
            , IMoveRepository moveRepository
            , IMoveDetailRepository moveDetailRepository)
            : base(repository, memoryCacheManager, mapper, validation, configuration)
        {
            this.moveRepository = moveRepository;
            this.moveDetailRepository = moveDetailRepository;
            _mapper = mapper;
        }

        public async Task<List<string>> CreateMove(FileDTO file)
        {
             var moveDetails = await this.moveRepository.CreateMove(file);
             List<MoveDetailDTO> list = _mapperDependency.Map<List<MoveDetailDTO>>(moveDetails.ToList());
             return GetMovingGrips(list);
        }

        public List<string> GetMovingGrips(List<MoveDetailDTO> listMoveDetailDTO)
        {
            List<string> days = new List<string>();
            int index = 0;

            //Recorremos la lista de detalles de mudanza
            for (int i = 1; i < listMoveDetailDTO.Count; i++)
            {
                //Obtenemos la cantidad de detalles
                int numElements = listMoveDetailDTO.OrderBy(o => o.Position)
                                                    .Select(x => x.Value)
                                                    .ToList().ElementAt(i);
                //Obtenemos el rango de la lista de detalles
                var elementsDay = listMoveDetailDTO.OrderBy(o => o.Position)
                                                    .Select(x => x.Value)
                                                    .ToList()
                                                    .GetRange(i + 1, numElements);
                i = i + numElements;
                index++;
                //Mostramos el resultado del caso
                days.Add("Caso #" + index + ": " + CalculateTripsDay(elementsDay));
            }
            return days;
        }

        private int CalculateTripsDay(List<int> elementsDay)
        {
            //Inicializamos la cantidad de viajes que se pueden hacer
            int travel = 0;

            //Recorremos la lista de elementos hasta que no quede ninguno
            while (elementsDay.Count > 0)
            {
                //Contador para cantidad de viajes
                var count = 1;
                //Peso en libras del viaje
                var size = 0;
                //Buscamos el elemento con peso mayor actual de la lista
                var max = elementsDay.Max();//30
                                            //Lo eliminamos de la lista para saber que ya fue utilizado
                elementsDay.Remove(max);
                //Verificamos si el elemento pesa menos de 50 libras, si la suma con otros elementos aún es menor a 50 libras y si aún hay elementos por utilizar
                while ((max < 50 && size < 50) && elementsDay.Count > 0)
                {
                    //Buscamos el elemento menor para relacionarlo con el elemento mayor actual que tengamos en memoria
                    var min = elementsDay.Min();//1
                                                //Removemos el elemento menor actual
                    elementsDay.Remove(min);
                    //Contamos la cantidad de elementos utilizados para en la bolsa para el viaje
                    count++;
                    //Calculamos el total que cree que lleva la supervisora con respecto al último elemento
                    size = max * count;
                }
                //Validamos que si que el tamaño o el máximo actual equivalga a más de 50 librar para generar un nuevo viaje
                if (size >= 50 || max >= 50)
                {
                    travel++;
                }
            }
            return travel;
        }
    }

    #region Validator

    public class MoveResourceValidator : BaseServiceValidator<Move, DistributorResource>
    {
        IStringLocalizer<GlobalResource> _globalLocalizer;

        public MoveResourceValidator(IStringLocalizer<DistributorResource> localizer,
            IStringLocalizer<GlobalResource> globalLocalizer) : base(localizer)
        {
            _globalLocalizer = globalLocalizer;

        }

        #region Insert Rules
        public override void LoadPreInsertRules()
        {


        }

        public override void LoadPostInsertRules()
        {

        }

        #endregion

        #region Update Rules

        public override void LoadPreUpdateRules()
        {
        }

        public override void LoadPostUpdateRules()
        {

        }

        #endregion

        #region Delete Rules

        public override void LoadPreDeleteRules()
        {

        }

        public override void LoadPostDeleteRules()
        {

        }

        #endregion

        #region Advanced Validations



        #endregion
    }

    #endregion
}
