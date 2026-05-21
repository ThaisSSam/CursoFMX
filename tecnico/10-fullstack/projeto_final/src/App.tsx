import React, { Suspense, lazy, useCallback, useEffect, useState } from 'react';
import AppLoadingFallback from './components/layout/AppLoadingFallback';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Toast from './components/notifications/Toast';
import { ToastContainer } from './components/notifications/ToastContainer';
import { ToastProvider } from './contexts/ToastContext';
import { CustomToastProvider } from './contexts/CustomToastContext';
import { DatabaseUnavailableModal } from './components/modals/DatabaseUnavailableModal';
import { ApiOfflineModal } from './components/modals/ApiOfflineModal';
import { PrimeiroAcessoModal } from './components/modals/PrimeiroAcessoModal';
import {
  definirStorageAuth,
  limparStorageAuth,
  tokenEstaExpirado,
  sair as sairAuth,
  mudarContexto,
} from './services/authService';
import { empresaStore } from './store/empresaStore';

const DashboardScreen = lazy(() => import('./screens/Dashboard'));
const LoginScreen = lazy(() => import('./screens/Login'));
const TelaRedefinirSenhaGestao = lazy(() => import('./screens/Login/RedefinirSenhaGestao'));
const CadastroScreen = lazy(() => import('./screens/Cadastro'));
const SuprimentosScreen = lazy(() => import('./screens/Suprimentos'));
const CadastroSolicitacaoScreen = lazy(() => import('./screens/Suprimentos/Compras/Solicitacoes'));
const ConsultasScreen = lazy(() => import('./screens/Suprimentos/Compras/Consultas'));
const RequisicoesScreen = lazy(() => import('./screens/Suprimentos/Compras/Requisicoes'));
const CadastrarProdutoServicoScreen = lazy(
  () => import('./screens/Suprimentos/Compras/CadastrarProdutoServico'),
);
const PedidoSolicitacaoScreen = lazy(() => import('./screens/Suprimentos/Pedidos/Solicitacoes'));
const AnaliseCentroCustoScreen = lazy(
  () => import('./screens/Suprimentos/Pedidos/AnaliseCentroCusto'),
);
const AnaliseAreaTecnicaScreen = lazy(
  () => import('./screens/Suprimentos/Pedidos/AnaliseAreaTecnica'),
);
const AlmoxarifadoScreen = lazy(() => import('./screens/Suprimentos/Pedidos/Almoxarifado'));
const ComprasScreen = lazy(() => import('./screens/Suprimentos/Pedidos/Compras'));
const PedidosScreen = lazy(() => import('./screens/Suprimentos/Compras/Pedidos'));
const TelaConsultaCotacoesListagem = lazy(() => import('./screens/Suprimentos/Compras/Cotacoes'));
const FluxoCotacaoGeralTela = lazy(
  () => import('./screens/Suprimentos/Compras/Cotacoes/FluxoCotacaoGeral'),
);
const CadastroGruposScreen = lazy(() => import('./screens/Suprimentos/Compras/CadastroGrupos'));
const CadastroSubgruposScreen = lazy(
  () => import('./screens/Suprimentos/Compras/CadastroSubgrupos'),
);
const CadastroFamiliasScreen = lazy(() => import('./screens/Suprimentos/Compras/CadastroFamilias'));
const AdministracaoScreen = lazy(() => import('./screens/Administracao'));
const EmpresasFiliaisScreen = lazy(() => import('./screens/Administracao/EmpresasFiliais'));
const EngenhariaScreen = lazy(() => import('./screens/Engenharia'));
const ModulosFuncionalidadesScreen = lazy(
  () => import('./screens/Administracao/ModulosFuncionalidades'),
);
const CriarModuloScreen = lazy(
  () => import('./screens/Administracao/ModulosFuncionalidades/CriarModulo'),
);
const EditarModuloScreen = lazy(
  () => import('./screens/Administracao/ModulosFuncionalidades/EditarModulo'),
);
const EditarFuncionalidadeScreen = lazy(
  () => import('./screens/Administracao/ModulosFuncionalidades/EditarFuncionalidade'),
);
const PerfisScreen = lazy(() => import('./screens/Administracao/Perfis'));
const CriarPerfilScreen = lazy(() => import('./screens/Administracao/Perfis/CriarPerfil'));
const EditarPerfilScreen = lazy(() => import('./screens/Administracao/Perfis/EditarPerfil'));
const VisualizarPerfilScreen = lazy(
  () => import('./screens/Administracao/Perfis/VisualizarPerfil'),
);
const UsuariosScreen = lazy(() => import('./screens/Administracao/Usuarios'));
const CadastroUsuarioScreen = lazy(
  () => import('./screens/Administracao/Usuarios/CadastroUsuario'),
);
const PessoasScreen = lazy(() => import('./screens/Administracao/Pessoas'));
const CadastroPessoaScreen = lazy(() => import('./screens/Administracao/Pessoas/CadastroPessoa'));
const TelaSelecaoEmpresa = lazy(() => import('./screens/Login/SelecaoEmpresa'));
const ConsultaGruposSubgruposScreen = lazy(
  () => import('./screens/Suprimentos/Compras/ConsultaGruposSubgrupos'),
);
const EditarGruposScreen = lazy(
  () => import('./screens/Suprimentos/Compras/ConsultaGruposSubgrupos/Edicao/EditarGrupo'),
);
const EditarSubgruposScreen = lazy(
  () => import('./screens/Suprimentos/Compras/ConsultaGruposSubgrupos/Edicao/EditarSubgrupo'),
);
const EditarFamiliasScreen = lazy(
  () => import('./screens/Suprimentos/Compras/ConsultaGruposSubgrupos/Edicao/EditarFamilia'),
);
const UnidadesMedidaScreen = lazy(() => import('./screens/Suprimentos/Compras/UnidadesMedida'));
const CadastroUnidadeMedidaScreen = lazy(
  () => import('./screens/Suprimentos/Compras/CadastroUnidadeMedida'),
);
const EdicaoUnidadeMedidaScreen = lazy(
  () => import('./screens/Suprimentos/Compras/EdicaoUnidadeMedida'),
);
const FinanceiroScreen = lazy(() => import('./screens/Financeiro'));
const ConsultaMoedasScreen = lazy(() => import('./screens/Financeiro/ConsultaMoedas'));
const CadastroMoedasScreen = lazy(
  () => import('./screens/Financeiro/ConsultaMoedas/Modals/ModalCadastro'),
);
const EdicaoMoedasScreen = lazy(
  () => import('./screens/Financeiro/ConsultaMoedas/Modals/ModalEdicao'),
);
const ConsultaBancosScreen = lazy(() => import('./screens/Financeiro/ConsultaBancos'));
const CadastroBancosScreen = lazy(
  () => import('./screens/Financeiro/ConsultaBancos/Modals/ModalCadastro'),
);
const EdicaoBancosScreen = lazy(
  () => import('./screens/Financeiro/ConsultaBancos/Modals/ModalEdicao'),
);
const OperacoesPcpScreen = lazy(() => import('./screens/OperacoesPcp'));
const ConsultaOperacoesScreen = lazy(
  () => import('./screens/OperacoesPcp/ConsultaOperacoesPadrao'),
);
const CadastroOperacoesScreen = lazy(
  () => import('./screens/OperacoesPcp/ConsultaOperacoesPadrao/Modals/ModalCadastro'),
);
const EdicaoOperacoesScreen = lazy(
  () => import('./screens/OperacoesPcp/ConsultaOperacoesPadrao/Modals/ModalEdicao'),
);
const ConsultaRecursosProdutivosScreen = lazy(
  () => import('./screens/OperacoesPcp/RecursoProdutivo/ConsultaRecursosProdutivos'),
);
const DetalheRecursoProdutivoScreen = lazy(
  () => import('./screens/OperacoesPcp/RecursoProdutivo/DetalheRecursoProdutivo'),
);
const FormularioRecursoProdutivoScreen = lazy(
  () => import('./screens/OperacoesPcp/RecursoProdutivo/FormularioRecursoProdutivo'),
);
const ConsultarEstruturaScreen = lazy(
  () => import('./screens/Engenharia/EstruturaProduto/ConsultaEstruturaProduto'),
);
const CriarEstruturaProdutoScreen = lazy(
  () => import('./screens/Engenharia/EstruturaProduto/CriarEstruturaProduto'),
);
const EditarEstruturaProdutoScreen = lazy(
  () => import('./screens/Engenharia/EstruturaProduto/EditarEstruturaProduto'),
);
const ConsultaComponentesEstruturaProdutoScreen = lazy(
  () => import('./screens/Engenharia/EstruturaProduto/ConsultaComponentesEstruturaProduto'),
);
const VisualizarEstruturaProdutoScreen = lazy(
  () => import('./screens/Engenharia/EstruturaProduto/VisualizarEstruturaProduto'),
);

const USAR_AUTH_MOCK = false;

interface LoginDadosComEmpresa {
  tokenAcesso: string;
  tokenRefresh?: string;
  expiraEm?: string;
  usuario?: unknown;
  ultimaEmpresaId: number | null;
  requiresPasswordChange?: boolean;
}

const AppRoutes = React.memo(() => {
  const [vrToken, fnSetToken] = useState(null);
  const [mostrarToastBemVindo, fnSetMostrarToastBemVindo] = useState(false);
  const [empresaId, setEmpresaId] = useState<number | null>(null);
  const [ehLoginNovo, setEhLoginNovo] = useState(false);
  const [processandoLogin, setProcessandoLogin] = useState(false);
  const [mostrarPrimeiroAcesso, setMostrarPrimeiroAcesso] = useState(false);
  const [dadosLoginPrimeiroAcesso, setDadosLoginPrimeiroAcesso] =
    useState<LoginDadosComEmpresa | null>(null);

  useEffect(() => {
    const unsubscribe = empresaStore.subscribe((state) => {
      const novoEmpresaId = state.empresaId;
      setEmpresaId((prevEmpresaId) => {
        if (prevEmpresaId !== novoEmpresaId) {
          return novoEmpresaId;
        }
        return prevEmpresaId;
      });
    });
    const estadoInicial = empresaStore.getState().empresaId;
    setEmpresaId(estadoInicial);
    return unsubscribe;
  }, []);

  useEffect(() => {
    if (empresaId !== null && ehLoginNovo) {
      setEhLoginNovo(false);
    }
  }, [empresaId, ehLoginNovo]);

  useEffect(() => {
    if (vrToken) {
      const estadoAtual = empresaStore.getState().empresaId;
      setEmpresaId(estadoAtual);
    } else {
      setEmpresaId(null);
    }
  }, [vrToken]);

  useEffect(() => {
    const tokenArmazenado = localStorage.getItem('auth_token');
    if (tokenArmazenado && !tokenEstaExpirado()) {
      fnSetToken(tokenArmazenado);
      empresaStore.getState().inicializarDoStorage();
      setEmpresaId(empresaStore.getState().empresaId);
      setEhLoginNovo(false);
    } else if (tokenArmazenado && tokenEstaExpirado()) {
      limparStorageAuth();
      empresaStore.getState().setEmpresaId(null);
      empresaStore.getState().setEmpresaDados(null);
      setEmpresaId(null);
      setEhLoginNovo(false);
      fnSetToken(null);
    }
  }, []);

  useEffect(() => {
    const deveMostrarToast = localStorage.getItem('show_welcome_toast');
    if (deveMostrarToast === '1') {
      fnSetMostrarToastBemVindo(true);
      localStorage.removeItem('show_welcome_toast');
    }
  }, []);

  useEffect(() => {
    const tratarNaoAutorizado = () => {
      limparStorageAuth();
      empresaStore.getState().setEmpresaId(null);
      empresaStore.getState().setEmpresaDados(null);
      fnSetToken(null);

      window.location.href = '/login';
    };
    window.addEventListener('auth:unauthorized', tratarNaoAutorizado);
    return () => window.removeEventListener('auth:unauthorized', tratarNaoAutorizado);
  }, []);

  const fnHandleLoginSucesso = useCallback(async (dados) => {
    if (typeof dados === 'string') {
      localStorage.setItem('auth_token', dados);
      localStorage.setItem('token', dados);
      empresaStore.getState().setEmpresaId(null);
      fnSetToken(dados);
      setEmpresaId(null);
      setEhLoginNovo(true);
    } else {
      definirStorageAuth(dados);

      if (dados.requiresPasswordChange) {
        setDadosLoginPrimeiroAcesso(dados);
        setMostrarPrimeiroAcesso(true);
        return;
      }

      if (dados.ultimaEmpresaId != null) {
        setProcessandoLogin(true);
        try {
          const resposta = await mudarContexto(dados.ultimaEmpresaId);
          if (resposta?.empresa) {
            empresaStore.getState().setEmpresaDados(resposta.empresa);
            empresaStore.getState().setEmpresaId(dados.ultimaEmpresaId);
            setEmpresaId(dados.ultimaEmpresaId);
            setEhLoginNovo(false);
          } else {
            empresaStore.getState().setEmpresaId(dados.ultimaEmpresaId);
            setEmpresaId(dados.ultimaEmpresaId);
            setEhLoginNovo(false);
          }
          fnSetToken(dados.tokenAcesso);
          fnSetMostrarToastBemVindo(true);
        } catch {
          empresaStore.getState().setEmpresaId(null);
          setEmpresaId(null);
          setEhLoginNovo(true);
          fnSetToken(dados.tokenAcesso);
        } finally {
          setProcessandoLogin(false);
        }
      } else {
        empresaStore.getState().setEmpresaId(null);
        setEmpresaId(null);
        setEhLoginNovo(true);
        fnSetToken(dados.tokenAcesso);
      }
    }
  }, []);
  const aoConcluirPrimeiroAcesso = useCallback(async () => {
    const dados = dadosLoginPrimeiroAcesso;
    setMostrarPrimeiroAcesso(false);
    if (!dados) return;

    if (dados.ultimaEmpresaId != null) {
      setProcessandoLogin(true);
      try {
        const resposta = await mudarContexto(dados.ultimaEmpresaId);
        if (resposta?.empresa) {
          empresaStore.getState().setEmpresaDados(resposta.empresa);
          empresaStore.getState().setEmpresaId(dados.ultimaEmpresaId);
          setEmpresaId(dados.ultimaEmpresaId);
          setEhLoginNovo(false);
        } else {
          empresaStore.getState().setEmpresaId(dados.ultimaEmpresaId);
          setEmpresaId(dados.ultimaEmpresaId);
          setEhLoginNovo(false);
        }
        fnSetToken(dados.tokenAcesso);
        fnSetMostrarToastBemVindo(true);
      } catch {
        empresaStore.getState().setEmpresaId(null);
        setEmpresaId(null);
        setEhLoginNovo(true);
        fnSetToken(dados.tokenAcesso);
      } finally {
        setProcessandoLogin(false);
      }
    } else {
      empresaStore.getState().setEmpresaId(null);
      setEmpresaId(null);
      setEhLoginNovo(true);
      fnSetToken(dados.tokenAcesso);
    }

    setDadosLoginPrimeiroAcesso(null);
  }, [dadosLoginPrimeiroAcesso]);

  const fnHandleSair = useCallback(async () => {
    try {
      await sairAuth();
    } finally {
      limparStorageAuth();
      empresaStore.getState().setEmpresaId(null);
      empresaStore.getState().setEmpresaDados(null);
      fnSetToken(null);
    }
  }, []);

  if (!vrToken) {
    return (
      <Suspense fallback={<AppLoadingFallback />}>
        <Routes>
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route
            path="/login"
            element={
              <LoginScreen usarAuthMock={USAR_AUTH_MOCK} onLoginSucesso={fnHandleLoginSucesso} />
            }
          />
          <Route path="/redefinir-senha-gestao" element={<TelaRedefinirSenhaGestao />} />
        </Routes>
      </Suspense>
    );
  }

  if (processandoLogin) {
    return null;
  }

  const precisaSelecionarEmpresa = empresaId === null || ehLoginNovo;

  if (precisaSelecionarEmpresa) {
    return (
      <Suspense fallback={<AppLoadingFallback />}>
        <>
          <TelaSelecaoEmpresa />
          <Toast
            title="Bem-vindo!"
            message="Bem-vindo ao sistema!"
            type="success"
            show={mostrarToastBemVindo}
            onClose={() => fnSetMostrarToastBemVindo(false)}
            duration={5000}
            icon={undefined}
          />
        </>
      </Suspense>
    );
  }

  return (
    <>
      <PrimeiroAcessoModal
        aberto={mostrarPrimeiroAcesso}
        login={((dadosLoginPrimeiroAcesso?.usuario ?? {}) as { name?: string }).name ?? ''}
        onSucesso={aoConcluirPrimeiroAcesso}
      />
      <Toast
        title="Bem-vindo!"
        message="Bem-vindo ao sistema!"
        type="success"
        show={mostrarToastBemVindo}
        onClose={() => fnSetMostrarToastBemVindo(false)}
        duration={5000}
        icon={undefined}
      />
      <Suspense fallback={<AppLoadingFallback />}>
        <Routes>
          <Route path="/" element={<Navigate to="/home" replace />} />
          <Route path="/home" element={<DashboardScreen onLogout={fnHandleSair} />} />

          <Route path="/engenharia" element={<EngenhariaScreen onLogout={fnHandleSair} />}>
            {/* <Route path="consulta-estrutura-produtos" element={<ConsultaEstruturaProdScreen />} />
          <Route path="editar-estrutura-produtos" element={<EditarEstruturaScreen />} />
          <Route path="visualizar-estrutura-produtos" element={<VisualizarEstruturaScreen />} /> */}
            <Route path="estruturas-produto" element={<ConsultarEstruturaScreen />} />
            <Route path="estruturas-produto/criar" element={<CriarEstruturaProdutoScreen />} />
            <Route
              path="estruturas-produto/editar/:id"
              element={<EditarEstruturaProdutoScreen />}
            />
            <Route
              path="estruturas-produto/visualizar/:id"
              element={<VisualizarEstruturaProdutoScreen />}
            />
            <Route
              path="estruturas-produto/:id/componentes"
              element={<ConsultaComponentesEstruturaProdutoScreen />}
            />
          </Route>

          <Route path="/suprimentos" element={<SuprimentosScreen onLogout={fnHandleSair} />}>
            <Route
              path="compras/solicitacao-produto-servico"
              element={<CadastroSolicitacaoScreen />}
            />
            <Route path="compras/consulta-solicitacoes" element={<ConsultasScreen />} />
            <Route path="compras/consulta-requisicoes" element={<RequisicoesScreen />} />
            <Route
              path="compras/requisicao-produtos-servicos"
              element={<PedidoSolicitacaoScreen />}
            />
            <Route path="pedidos/analise-centro-custo" element={<AnaliseCentroCustoScreen />} />
            <Route path="pedidos/analise-area-tecnica" element={<AnaliseAreaTecnicaScreen />} />
            <Route path="pedidos/almoxarifado" element={<AlmoxarifadoScreen />} />
            <Route path="pedidos/compras" element={<ComprasScreen />} />
            <Route
              path="compras/cadastrar-produto-servico"
              element={<CadastrarProdutoServicoScreen />}
            />
            <Route path="compras/consulta-cadastros" element={<PedidosScreen />} />
            <Route path="compras/consulta-cotacoes" element={<TelaConsultaCotacoesListagem />} />
            <Route path="compras/cotacao-geral/*" element={<FluxoCotacaoGeralTela />} />
            <Route path="compras/grupos-subgrupos" element={<ConsultaGruposSubgruposScreen />} />
            <Route path="compras/cadastro-grupos" element={<CadastroGruposScreen />} />
            <Route path="compras/cadastro-subgrupo" element={<CadastroSubgruposScreen />} />
            <Route path="compras/cadastro-familias" element={<CadastroFamiliasScreen />} />
          </Route>
          <Route path="/suprimentos" element={<SuprimentosScreen onLogout={fnHandleSair} />} />
          <Route
            path="compras/solicitacao-produto-servico"
            element={<CadastroSolicitacaoScreen />}
          />
          <Route path="compras/consulta-solicitacoes" element={<ConsultasScreen />} />
          <Route path="compras/consulta-requisicoes" element={<RequisicoesScreen />} />
          <Route
            path="compras/requisicao-produtos-servicos"
            element={<PedidoSolicitacaoScreen />}
          />
          <Route path="pedidos/analise-centro-custo" element={<AnaliseCentroCustoScreen />} />
          <Route path="pedidos/analise-area-tecnica" element={<AnaliseAreaTecnicaScreen />} />
          <Route path="pedidos/almoxarifado" element={<AlmoxarifadoScreen />} />
          <Route path="pedidos/compras" element={<ComprasScreen />} />
          <Route
            path="compras/cadastrar-produto-servico"
            element={<CadastrarProdutoServicoScreen />}
          />
          <Route path="compras/consulta-cadastros" element={<PedidosScreen />} />
          <Route path="compras/consulta-cotacoes" element={<TelaConsultaCotacoesListagem />} />
          <Route path="compras/cotacao-geral/*" element={<FluxoCotacaoGeralTela />} />
          <Route path="compras/grupos-subgrupos" element={<ConsultaGruposSubgruposScreen />} />
          <Route path="compras/cadastro-grupos" element={<CadastroGruposScreen />} />
          <Route path="compras/editar-grupo" element={<EditarGruposScreen />} />
          <Route path="compras/cadastro-subgrupo" element={<CadastroSubgruposScreen />} />
          <Route path="compras/editar-subgrupo" element={<EditarSubgruposScreen />} />
          <Route path="compras/editar-familia" element={<EditarFamiliasScreen />} />
          <Route path="/suprimentos" element={<SuprimentosScreen onLogout={fnHandleSair} />}>
            <Route
              path="compras/solicitacao-produto-servico"
              element={<CadastroSolicitacaoScreen />}
            />
            <Route path="compras/consulta-solicitacoes" element={<ConsultasScreen />} />
            <Route path="compras/consulta-requisicoes" element={<RequisicoesScreen />} />
            <Route
              path="compras/requisicao-produtos-servicos"
              element={<PedidoSolicitacaoScreen />}
            />
            <Route path="pedidos/analise-centro-custo" element={<AnaliseCentroCustoScreen />} />
            <Route path="pedidos/analise-area-tecnica" element={<AnaliseAreaTecnicaScreen />} />
            <Route path="pedidos/almoxarifado" element={<AlmoxarifadoScreen />} />
            <Route path="pedidos/compras" element={<ComprasScreen />} />
            <Route
              path="compras/cadastrar-produto-servico"
              element={<CadastrarProdutoServicoScreen />}
            />
            <Route path="compras/consulta-cadastros" element={<PedidosScreen />} />
            <Route path="compras/consulta-cotacoes" element={<TelaConsultaCotacoesListagem />} />
            <Route path="compras/cotacao-geral/*" element={<FluxoCotacaoGeralTela />} />
            <Route path="compras/grupos-subgrupos" element={<ConsultaGruposSubgruposScreen />} />
            <Route path="compras/cadastro-grupos" element={<CadastroGruposScreen />} />
            <Route path="compras/editar-grupo" element={<EditarGruposScreen />} />
            <Route path="compras/cadastro-subgrupo" element={<CadastroSubgruposScreen />} />
            <Route path="compras/editar-subgrupo" element={<EditarSubgruposScreen />} />
            <Route path="compras/editar-familia" element={<EditarFamiliasScreen />} />

            <Route path="compras/unidades-medida" element={<UnidadesMedidaScreen />} />
            <Route
              path="compras/cadastro-unidades-medida"
              element={<CadastroUnidadeMedidaScreen />}
            />
            <Route path="compras/editar-unidades-medida" element={<EdicaoUnidadeMedidaScreen />} />
          </Route>

          <Route path="/operacoespcp" element={<OperacoesPcpScreen onLogout={fnHandleSair} />}>
            <Route path="consulta-operacoes" element={<ConsultaOperacoesScreen />} />
            <Route path="cadastro-operacoes" element={<CadastroOperacoesScreen />} />
            <Route path="editar-operacoes" element={<EdicaoOperacoesScreen />} />
            <Route path="recursos-produtivos" element={<ConsultaRecursosProdutivosScreen />} />
            <Route
              path="recursos-produtivos/visualizar/:id"
              element={<DetalheRecursoProdutivoScreen />}
            />
            <Route
              path="recursos-produtivos/editar/:id"
              element={<FormularioRecursoProdutivoScreen />}
            />
            <Route
              path="recursos-produtivos/cadastro/:tipo"
              element={<FormularioRecursoProdutivoScreen />}
            />
          </Route>

          <Route path="/financeiro" element={<FinanceiroScreen onLogout={fnHandleSair} />}>
            <Route path="consulta-moedas" element={<ConsultaMoedasScreen />} />
            <Route path="cadastro-moeda" element={<CadastroMoedasScreen />} />
            <Route path="editar-moeda" element={<EdicaoMoedasScreen />} />
            <Route path="consulta-bancos" element={<ConsultaBancosScreen />} />
            <Route path="cadastro-banco" element={<CadastroBancosScreen />} />
            <Route path="editar-banco" element={<EdicaoBancosScreen />} />
          </Route>

          <Route path="/cadastro" element={<CadastroScreen onLogout={fnHandleSair} />} />
          <Route path="/administracao" element={<AdministracaoScreen onLogout={fnHandleSair} />}>
            <Route path="empresas-filiais" element={<EmpresasFiliaisScreen />} />
            <Route path="modulos-funcionalidades" element={<ModulosFuncionalidadesScreen />} />
            <Route path="modulos-funcionalidades/criar" element={<CriarModuloScreen />} />
            <Route path="modulos-funcionalidades/editar/:id" element={<EditarModuloScreen />} />
            <Route
              path="modulos-funcionalidades/funcionalidade/editar/:id"
              element={<EditarFuncionalidadeScreen />}
            />
            <Route path="perfis" element={<PerfisScreen />} />
            <Route path="perfis/criar" element={<CriarPerfilScreen />} />
            <Route path="perfis/editar/:id" element={<EditarPerfilScreen />} />
            <Route path="perfis/visualizar/:id" element={<VisualizarPerfilScreen />} />
            <Route path="usuarios" element={<UsuariosScreen />} />
            <Route path="usuarios/cadastro" element={<CadastroUsuarioScreen />} />
            <Route path="pessoas" element={<PessoasScreen />} />
            <Route path="pessoas/cadastro" element={<CadastroPessoaScreen />} />
            <Route path="pessoas/cadastro/:pessoaId" element={<CadastroPessoaScreen />} />
          </Route>
          <Route path="*" element={<Navigate to="/home" replace />} />
        </Routes>
      </Suspense>
    </>
  );
});

AppRoutes.displayName = 'AppRoutes';

function AppContent() {
  return (
    <>
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
      <ToastContainer />

      <DatabaseUnavailableModal />
      <ApiOfflineModal />
    </>
  );
}

function App() {
  return (
    <ToastProvider>
      <CustomToastProvider>
        <AppContent />
      </CustomToastProvider>
    </ToastProvider>
  );
}

export default App;
