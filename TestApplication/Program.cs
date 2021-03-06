﻿using Autofac;
using AutoMapper;
using ExamGenerator.DocumentManager;
using ExamGenerator.DocumentManager.QRCodeGenerator;
using ExamGenerator.DocumentManager.UnZipArchive;
using ExamGenerator.Service.Interfaces;
using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<DTOProfile>());
            ExamGeneratorDBContext cont = new ExamGeneratorDBContext();
            AnswerService serviceA = new AnswerService(cont);
            QuestionService serviceQ = new QuestionService(cont);
            ExamCoreService serviceE = new ExamCoreService(cont, serviceA, serviceQ);
            AnswerPositionService serviceAP = new AnswerPositionService(cont);

            var bitmaps = ArchiveUnZiper.GetBitmapsFromZipArchive("sprawdziany.zip");
            var validator = new DocumentValidator(bitmaps);
            var examIDs = validator.GetExamIDs();

            foreach (var item in examIDs)
            {
                var egzaminAP = serviceAP.GetAllAnswersPositionsByExamID(item);
                validator.CheckExam(item, Mapper.Map<List<AnswerPositionDTO>>(egzaminAP));
                Console.WriteLine();
            }
            Console.Read();
        }
    }

    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();

            CreateMap<Question, QuestionDTO>()
                .ForMember(
                destination => destination.AnswersDTO,
               opts => opts.MapFrom(source => source.Answers));
            CreateMap<QuestionDTO, Question>();

            CreateMap<ExamCore, ExamDTO>()
                .ForMember(destination => destination.QuestionsDTO, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamDTO, ExamCore>()
                .ForMember(destination => destination.Questions, opts => opts.Ignore());

            CreateMap<AnswerPosition, AnswerPositionDTO>()
                 .ForMember(destination => destination.AnswerDTO, opts => opts.MapFrom(source => source.Answer)); ;
            CreateMap<AnswerPositionDTO, AnswerPosition>();
        }
    }
}
