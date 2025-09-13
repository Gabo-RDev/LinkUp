"use client"
import { useState } from "react"
import { SlidersHorizontal, X } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"

interface AdvancedFiltersProps {
  onFiltersChange: (filters: FilterState) => void
  categories: Array<{ id: string; name: string }>
}

interface FilterState {
  sortBy: string
  dateRange: string
  categories: string[]
}

export function AdvancedFilters({ onFiltersChange, categories }: AdvancedFiltersProps) {
  const [filters, setFilters] = useState<FilterState>({
    sortBy: "newest",
    dateRange: "all",
    categories: [],
  })
  const [isOpen, setIsOpen] = useState(false)

  const updateFilters = (newFilters: Partial<FilterState>) => {
    const updatedFilters = { ...filters, ...newFilters }
    setFilters(updatedFilters)
    onFiltersChange(updatedFilters)
  }

  const toggleCategory = (categoryId: string) => {
    const newCategories = filters.categories.includes(categoryId)
      ? filters.categories.filter((id) => id !== categoryId)
      : [...filters.categories, categoryId]

    updateFilters({ categories: newCategories })
  }

  const clearFilters = () => {
    const clearedFilters = {
      sortBy: "newest",
      dateRange: "all",
      categories: [],
    }
    setFilters(clearedFilters)
    onFiltersChange(clearedFilters)
  }

  const hasActiveFilters = filters.sortBy !== "newest" || filters.dateRange !== "all" || filters.categories.length > 0

  return (
    <div className="flex items-center gap-4">
      {/* Active Filters Display */}
      {hasActiveFilters && (
        <div className="flex items-center gap-2">
          {filters.categories.map((categoryId) => {
            const category = categories.find((c) => c.id === categoryId)
            return category ? (
              <Badge key={categoryId} variant="secondary" className="gap-1">
                {category.name}
                <X className="h-3 w-3 cursor-pointer" onClick={() => toggleCategory(categoryId)} />
              </Badge>
            ) : null
          })}
          {filters.sortBy !== "newest" && (
            <Badge variant="secondary">
              {filters.sortBy === "oldest"
                ? "Más antiguos"
                : filters.sortBy === "popular"
                  ? "Más populares"
                  : "Más recientes"}
            </Badge>
          )}
          {filters.dateRange !== "all" && (
            <Badge variant="secondary">
              {filters.dateRange === "week" ? "Esta semana" : filters.dateRange === "month" ? "Este mes" : "Este año"}
            </Badge>
          )}
          <Button variant="ghost" size="sm" onClick={clearFilters}>
            Limpiar filtros
          </Button>
        </div>
      )}

      {/* Filters Popover */}
      <Popover open={isOpen} onOpenChange={setIsOpen}>
        <PopoverTrigger asChild>
          <Button variant="outline" size="sm" className="gap-2 bg-transparent">
            <SlidersHorizontal className="h-4 w-4" />
            Filtros
          </Button>
        </PopoverTrigger>
        <PopoverContent className="w-80" align="end">
          <Card className="border-0 shadow-none">
            <CardHeader className="pb-4">
              <CardTitle className="text-lg">Filtros Avanzados</CardTitle>
            </CardHeader>
            <CardContent className="space-y-6">
              {/* Sort By */}
              <div className="space-y-2">
                <label className="text-sm font-medium">Ordenar por</label>
                <Select value={filters.sortBy} onValueChange={(value) => updateFilters({ sortBy: value })}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="newest">Más recientes</SelectItem>
                    <SelectItem value="oldest">Más antiguos</SelectItem>
                    <SelectItem value="popular">Más populares</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              {/* Date Range */}
              <div className="space-y-2">
                <label className="text-sm font-medium">Fecha de publicación</label>
                <Select value={filters.dateRange} onValueChange={(value) => updateFilters({ dateRange: value })}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="all">Cualquier fecha</SelectItem>
                    <SelectItem value="week">Esta semana</SelectItem>
                    <SelectItem value="month">Este mes</SelectItem>
                    <SelectItem value="year">Este año</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              {/* Categories */}
              <div className="space-y-2">
                <label className="text-sm font-medium">Categorías</label>
                <div className="flex flex-wrap gap-2 max-h-32 overflow-y-auto">
                  {categories.map((category) => (
                    <Badge
                      key={category.id}
                      variant={filters.categories.includes(category.id) ? "default" : "outline"}
                      className="cursor-pointer"
                      onClick={() => toggleCategory(category.id)}
                    >
                      {category.name}
                    </Badge>
                  ))}
                </div>
              </div>

              {/* Actions */}
              <div className="flex justify-between pt-4">
                <Button variant="outline" onClick={clearFilters} disabled={!hasActiveFilters}>
                  Limpiar
                </Button>
                <Button onClick={() => setIsOpen(false)}>Aplicar</Button>
              </div>
            </CardContent>
          </Card>
        </PopoverContent>
      </Popover>
    </div>
  )
}
