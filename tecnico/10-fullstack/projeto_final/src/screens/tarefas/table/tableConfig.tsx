import { type ColumnDef } from '@tanstack/react-table';
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Eye, PenBox, Trash2 } from 'lucide-react';
import type { Tarefa } from "@/services/endpoints/tarefas";

export const DEFAULT_TAREFA_COLUMNS = [
  { id: 'select', label: 'Seleção', visible: true, fixed: true },
  { id: 'acoes', label: 'Ações', visible: true, fixed: true },
  { id: 'codigo', label: 'Código', visible: true },
  { id: 'nome', label: 'Título', visible: true },
  { id: 'responsavel', label: 'Responsável', visible: true },
  { id: 'prioridade', label: 'Prioridade', visible: true },
  { id: 'situacao', label: 'Situação', visible: true },
  { id: 'dataCriacao', label: 'Data Criação', visible: true },
];

const getPrioridadeConfig = (prio: unknown) => {
  const valorTexto = String(prio ?? "").toLowerCase().trim();

  switch (valorTexto) {
    case "alta":
      return { label: "Alta", classes: "bg-red-500/10 text-red-500 border-red-500/20" };    
    case "media":
      return { label: "Média", classes: "bg-amber-500/10 text-amber-500 border-amber-500/20" };
    default:
      return { label: "Baixa", classes: "bg-slate-500/10 text-slate-400 border-slate-500/20" };
  }
};

const getSituacaoConfig = (sit: unknown) => {
  const valorTexto = String(sit ?? "").trim();
  const valorLower = valorTexto.toLowerCase();

  switch (valorLower) {
    case "afazer":
    case "a fazer":
      return { label: "A fazer", classes: "bg-slate-700 text-slate-300 border-slate-600" };
    case "emandamento":
    case "em andamento":
      return { label: "Em andamento", classes: "bg-blue-500/10 text-blue-400 border-blue-500/20" };
    case "concluido":
    case "concluida":
    case "concluída":
      return { label: "Concluída", classes: "bg-emerald-500/10 text-emerald-400 border-emerald-500/20" };
    case "bloqueada":
      return { label: "Bloqueada", classes: "bg-rose-500/10 text-rose-500 border-rose-500/20" };
    case "emvalidacao":
    case "em validação":
      return { label: "Em validação", classes: "bg-purple-500/10 text-purple-400 border-purple-500/20" };
    default:
      return { label: valorTexto || "Backlog", classes: "bg-slate-800 text-slate-400 border-slate-700" };
  }
};

export const createTarefaColumns = (
  onViewClick?: (data: Tarefa) => void,
  onEditClick?: (data: Tarefa) => void,
  onDeleteClick?: (data: Tarefa) => void,
): ColumnDef<Tarefa>[] => [
  {
    id: 'select',
    header: ({ table }) => (
      <Checkbox 
        checked={table.getIsAllRowsSelected()} 
        onCheckedChange={(value) => table.toggleAllRowsSelected(!!value)}
        className="border-slate-700 data-[state=checked]:bg-blue-600"
      />
    ),
    cell: ({ row }) => (
      <Checkbox 
        checked={row.getIsSelected()} 
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        className="border-slate-700 data-[state=checked]:bg-blue-600"
      />
    ),
    size: 50,
    enableSorting: false,
    enableResizing: false,
  },
  {
    id: 'acoes',
    header: 'Ações',
    cell: ({ row }) => {
      const isExcluido = (row.original as any).excluido; 

      return (
        <div className="flex gap-1 justify-center">
          {!isExcluido && (
            <Button 
              variant="ghost" 
              size="icon" 
              className="h-7 w-7 text-slate-500 hover:text-slate-300 cursor-pointer"
              onClick={() => onEditClick?.(row.original)}
            >
              <PenBox size={14} />
            </Button>
          )}

          <Button 
            variant="ghost" 
            size="icon" 
            className="h-7 w-7 text-slate-500 hover:text-slate-300 cursor-pointer"
            onClick={() => onViewClick?.(row.original)}
          >
            <Eye size={14} />
          </Button>

          {!isExcluido && (
            <Button 
              variant="ghost" 
              size="icon" 
              className="h-7 w-7 text-rose-500 hover:text-rose-400 hover:bg-rose-500/10 cursor-pointer"
              onClick={() => onDeleteClick?.(row.original)}
            >
              <Trash2 size={14} />
            </Button>
          )}

          {isExcluido && (
            <span className="text-[10px] text-rose-500 font-semibold px-1 py-1 select-none pt-1.5">Excluída</span>
          )}
        </div>
      );
    },
    size: 120,
    enableResizing: false,
  },
  {
    id: 'codigo',
    accessorKey: 'codigo',
    header: 'Código',
    cell: ({ row, getValue }) => (
      <div className={`font-mono font-medium block truncate ${
        (row.original as any).excluido ? "opacity-40 text-slate-500" : "text-slate-500"
      }`}>
        TASK-{String(getValue()).padStart(4, '0')}
      </div>
    ),
    size: 100,
    minSize: 90
  },
  {
    id: 'nome',
    accessorKey: 'nome',
    header: 'Título',
    cell: ({ row, getValue }) => (
      <span className={`font-medium max-w-xs block truncate ${
        (row.original as any).excluido ? "opacity-40 text-slate-500" : "text-slate-100"
      }`}>
        {String(getValue())}
      </span>
    ),
    size: 300,
    minSize: 90
  },
  {
    id: 'responsavel',
    accessorKey: 'responsavel',
    header: 'Responsável',
    cell: ({ row }) => {
      const resp = row.original.responsavel;
      const nomeResponsavel = resp?.email ? resp.email.split('@')[0] : "Sem dono";
      const iniciais = nomeResponsavel.substring(0, 2).toUpperCase();
      const isExcluido = (row.original as any).excluido;

      return (
        <div className={`flex items-center gap-2 block truncate ${isExcluido ? "opacity-40 text-slate-500" : ""}`}>
          <Avatar className="w-5 h-5 text-[9px] font-bold">
            <AvatarFallback className={`font-extrabold ${isExcluido ? "bg-slate-800 text-slate-500" : "bg-blue-600/20 text-blue-400"}`}>
              {iniciais}
            </AvatarFallback>
          </Avatar>
          <span className={`capitalize ${isExcluido && "text-slate-300"}`}>{nomeResponsavel}</span>
        </div>
      );
    },
    size: 170,
    minSize: 90
  },
  {
    id: 'prioridade',
    accessorKey: 'prioridade',
    header: 'Prioridade',
    cell: ({ row, getValue }) => {
      const config = getPrioridadeConfig(getValue());
      const isExcluido = row.original.excluido;
      return (
        <Badge 
          variant="outline" 
          className={`block truncate text-[10px] px-2 py-0.5 font-semibold rounded-md ${
            isExcluido ? "bg-slate-900/40 text-slate-600 border-slate-800/40 opacity-40" : config.classes
          }`}
        >
          {config.label}
        </Badge>
      );
    },
    size: 100,
    minSize: 90
  },
  {
    id: 'situacao',
    accessorKey: 'situacao',
    header: 'Situação',
    cell: ({ row, getValue }) => {
      const config = getSituacaoConfig(getValue());
      const isExcluido = row.original.excluido;
      return (
        <Badge 
          variant="outline" 
          className={`text-[10px] px-2 py-0.5 font-medium rounded-md block truncate ${
            isExcluido ? "bg-slate-900/40 text-slate-600 border-slate-800/40 opacity-40" : config.classes
          }`}
        >
          {config.label}
        </Badge>
      );
    },
    size: 120,
    minSize: 120,
  },
  {
    id: 'dataCriacao',
    accessorKey: 'dataCriacao',
    header: 'Data Criação',
    cell: ({ row, getValue }) => (
      <span className={`block truncate ${
        (row.original as any).excluido ? "opacity-40 text-slate-500" : "text-slate-400"
      }`}>
        {new Date(String(getValue())).toLocaleDateString('pt-BR')}
      </span>
    ),
    size: 120,
    minSize: 100
  },
];