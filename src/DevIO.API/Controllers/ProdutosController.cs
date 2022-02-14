using AutoMapper;
using DevIO.API.Dtos;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Controllers
{
    [Route("api/[controller]")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoRepository produtoRepository,
                                  IProdutoService produtoService, 
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<ProdutoDto>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDto>>(await _produtoRepository.ObterProdutosFornecedores());
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> ObterPorId(Guid id)
        {
            var produtoDto = await ObterProduto(id);
            if(produtoDto == null) return NotFound();
            return produtoDto;
        }
        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Adicionar(ProdutoDto produtoDto)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoDto));
            return CustomResponse(produtoDto);

        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> Exckuir(Guid id)
        {
            var produto = await ObterProduto(id);
            if(produto == null) return NotFound();
            await _produtoService.Remover(id);
            return CustomResponse(produto);
        }
        private bool UploadArquivo(string arquivo, string imgNome)
        {
            var imageDataByteArray = Convert.FromBase64String(arquivo);
            if(string.IsNullOrEmpty(arquivo))
            {                
                NotificarErro("Forneça uma imagem para este produto");
                return false;
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);
            if(System.IO.File.Exists(filePath))
            {                
                NotificarErro("Já existe um arquivo com esse nome");
                return false;
            }
            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);
            return true;
        }

        private async Task<ProdutoDto> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterProdutoFornecedor(id));
        }
        
    }
}