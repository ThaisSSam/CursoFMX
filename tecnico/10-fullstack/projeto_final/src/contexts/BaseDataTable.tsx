import React, { useState, useEffect, useRef } from 'react';
import { type Table as TanStackTable, flexRender, type Column } from '@tanstack/react-table';
import { ChevronLeft, ChevronRight, ChevronDown } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';

interface BaseDataTableProps<TData> {
  table: TanStackTable<TData>;
  isLoading?: boolean;
  enablePagination?: boolean;
  enableServerSidePagination?: boolean;
  enableColumnPinning?: boolean;
  enableColumnResizing?: boolean;
  columnLabels?: Record<string, string>;
  alturaAutomatica?: boolean;
  rolagemHorizontalExterna?: boolean;
}

const getPinningStyles = <TData,>(column: Column<TData>, enabled: boolean): React.CSSProperties => {
  if (!enabled) return {};
  const isPinned = column.getIsPinned();
  if (!isPinned) return { position: 'relative' };
  const startPosition = column.getStart(isPinned);
  return {
    position: 'sticky',
    left: isPinned === 'left' ? `${startPosition}px` : undefined,
    right: isPinned === 'right' ? `${startPosition}px` : undefined,
    zIndex: isPinned ? 2 : 1,
    backgroundColor: '#131b2e',
  };
};

export function BaseDataTable<TData>({
  table,
  isLoading = false,
  enablePagination = true,
  enableColumnPinning = false,
  enableColumnResizing = false,
  alturaAutomatica = false,
  rolagemHorizontalExterna = false,
}: BaseDataTableProps<TData>) {
  const rows = table.getRowModel().rows;
  const headerGroups = table.getHeaderGroups();
  const dropdownRef = useRef<HTMLDivElement>(null);
  const [isPageSizeDropdownOpen, setIsPageSizeDropdownOpen] = useState(false);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsPageSizeDropdownOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const classeRaizTabela = alturaAutomatica
    ? 'w-full flex flex-col'
    : 'w-full h-full flex flex-col';

  const classeCorpoRolagem = rolagemHorizontalExterna
    ? alturaAutomatica
      ? 'max-w-full overflow-x-visible overflow-y-visible'
      : 'flex-1 min-h-0 max-w-full overflow-x-visible overflow-y-visible'
    : alturaAutomatica
      ? 'scrollbar-thin overflow-x-auto'
      : 'scrollbar-thin flex-1 min-h-0 overflow-x-auto overflow-y-auto';

  return (
    <div className={classeRaizTabela}>      
      <div className={classeCorpoRolagem}>
        <Table 
          className="w-full border-separate border-spacing-0"
          style={{
            tableLayout: enableColumnResizing ? 'fixed' : 'auto',
            width: enableColumnResizing ? table.getTotalSize() : '100%',
          }}
        >
          <TableHeader className="border-b border-r border-slate-800 sticky top-0 z-10 bg-[#111827]">
            {headerGroups.map((headerGroup) => (
              <TableRow key={headerGroup.id} >
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead
                      key={header.id}
                      className="text-left text-xs font-semibold text-slate-400 border-b border-r border-slate-800 py-3 px-4 uppercase tracking-wider whitespace-nowrap relative group truncate"
                      style={{ 
                        width: header.getSize(),
                        ...getPinningStyles(header.column, enableColumnPinning)
                      }}
                    >
                      {flexRender(header.column.columnDef.header, header.getContext())}

                      {enableColumnResizing && header.column.getCanResize() && (
                        <div
                          onMouseDown={header.getResizeHandler()}
                          onTouchStart={header.getResizeHandler()}
                          className={`absolute right-0 top-0 h-full w-1 bg-blue-500/40 cursor-col-resize select-none touch-none opacity-0 group-hover:opacity-100 transition-opacity truncate ${
                            header.column.getIsResizing() ? 'bg-blue-500 opacity-100 w-1' : ''
                          }`}
                        />
                      )}
                    </TableHead>
                  );
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={headerGroups[0]?.headers.length || 1} className="text-center py-8 text-slate-500 border-b border-r border-slate-800">
                  Carregando tarefas...
                </TableCell>
              </TableRow>
            ) : rows?.length ? (
              rows.map((row) => {
                return (
                  <TableRow
                    key={row.id}
                    className="border-b border-r border-slate-800/60 hover:bg-[#1e293b]/30 transition-colors text-xs text-slate-300"
                  >
                    {row.getVisibleCells().map((cell) => {
                      return (
                        <TableCell
                          key={cell.id}
                          className="py-3 px-4 border-b border-r border-slate-800/60 whitespace-nowrap"
                          style={{
                            width: cell.column.getSize(),
                            ...getPinningStyles(cell.column, enableColumnPinning)
                          }}
                        >
                          {flexRender(cell.column.columnDef.cell, cell.getContext())}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })
            ) : (
              <TableRow>
                <TableCell colSpan={headerGroups[0]?.headers.length || 1} className="text-center py-10 text-slate-500 border-b border-r border-slate-800">
                  Nenhum resultado encontrado.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      {enablePagination && (
        <div className="flex-shrink-0 flex items-center justify-between px-4 bg-[#111827]/20 border-t border-slate-800 h-12">
          <div className="flex-1 text-xs text-slate-500">
            {table.getFilteredSelectedRowModel().rows.length > 0 && (
              <>
                {table.getFilteredSelectedRowModel().rows.length} de{" "}
                {table.getFilteredRowModel().rows.length} linha(s) selecionada(s).
              </>
            )}
          </div>
          <div className="flex items-center space-x-6">
            <div className="flex items-center space-x-2 text-xs text-slate-400">
              <p className="font-medium">Linhas por página</p>
              <div className="relative" ref={dropdownRef}>
                <button
                  onClick={() => setIsPageSizeDropdownOpen(!isPageSizeDropdownOpen)}
                  className="flex items-center gap-1 bg-[#131b2e] border border-slate-800 rounded px-2 py-1 text-slate-300 outline-none cursor-pointer"
                >
                  {table.getState().pagination.pageSize}
                  <ChevronDown size={12} />
                </button>
                {isPageSizeDropdownOpen && (
                  <div className="absolute bottom-full right-0 mb-1 bg-[#131b2e] border border-slate-800 rounded shadow-lg z-50 py-1 min-w-[60px]">
                    {[10, 20, 30, 50].map((size) => (
                      <button
                        key={size}
                        onClick={() => {
                          table.setPageSize(size);
                          setIsPageSizeDropdownOpen(false);
                        }}
                        className="w-full text-left px-2 py-1 hover:bg-slate-800 text-slate-300 text-xs "
                      >
                        {size} linhas
                      </button>
                    ))}
                  </div>
                )}
              </div>
            </div>
            <div className="text-xs font-medium text-slate-400">
              Página {table.getState().pagination.pageIndex + 1} de {table.getPageCount()}
            </div>
            <div className="flex items-center space-x-2">
              <Button
                variant="outline"
                size="icon"
                className="w-7 h-7 bg-[#131b2e] border-slate-800 text-slate-400 hover:bg-slate-800 cursor-pointer"
                onClick={() => table.previousPage()}
                disabled={!table.getCanPreviousPage()}
              >
                <ChevronLeft className="h-4 w-4" />
              </Button>
              <Button
                variant="outline"
                size="icon"
                className="w-7 h-7 bg-[#131b2e] border-slate-800 text-slate-400 hover:bg-slate-800 cursor-pointer"
                onClick={() => table.nextPage()}
                disabled={!table.getCanNextPage()}
              >
                <ChevronRight className="h-4 w-4" />
              </Button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}