"use client"
import { useState, useEffect, Suspense } from "react"
import { useSearchParams } from "next/navigation"
import { PostCard } from "@/components/post-card"
import { CategoryFilter } from "@/components/category-filter"
import { SearchPosts } from "@/components/search-posts"
import { AdvancedFilters } from "@/components/advanced-filters"
import { PostsLoading } from "@/components/posts-loading"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"

interface Post {
  id: string
  title: string
  content: string
  category_name: string
  category_id: string
  author_name: string
  author_username: string
  author_photo?: string
  likes_count: number
  created_at: string
}

interface Category {
  id: string
  name: string
}

function PostsContent() {
  const searchParams = useSearchParams()
  const [posts, setPosts] = useState<Post[]>([])
  const [categories, setCategories] = useState<Category[]>([])
  const [loading, setLoading] = useState(true)
  const [selectedCategory, setSelectedCategory] = useState<string>()
  const [searchQuery, setSearchQuery] = useState("")
  const [page, setPage] = useState(1)
  const [hasMore, setHasMore] = useState(true)

  useEffect(() => {
    const categoryFromUrl = searchParams.get("category")
    const searchFromUrl = searchParams.get("search")

    if (categoryFromUrl) setSelectedCategory(categoryFromUrl)
    if (searchFromUrl) setSearchQuery(searchFromUrl)
  }, [searchParams])

  useEffect(() => {
    loadCategories()
  }, [])

  useEffect(() => {
    loadPosts(true)
  }, [selectedCategory, searchQuery])

  const loadCategories = async () => {
    try {
      const response = await fetch("/api/categories")
      const data = await response.json()
      setCategories(data.categories || [])
    } catch (error) {
      console.error("Error loading categories:", error)
    }
  }

  const loadPosts = async (reset = false) => {
    try {
      setLoading(true)
      const currentPage = reset ? 1 : page

      const params = new URLSearchParams({
        page: currentPage.toString(),
        limit: "12",
      })

      if (selectedCategory) params.append("category", selectedCategory)
      if (searchQuery) params.append("search", searchQuery)

      const response = await fetch(`/api/posts?${params}`)
      const data = await response.json()

      if (reset) {
        setPosts(data.posts || [])
        setPage(2)
      } else {
        setPosts((prev) => [...prev, ...(data.posts || [])])
        setPage((prev) => prev + 1)
      }

      setHasMore(data.pagination?.hasMore || false)
    } catch (error) {
      console.error("Error loading posts:", error)
    } finally {
      setLoading(false)
    }
  }

  const handleCategoryChange = (categoryId?: string) => {
    setSelectedCategory(categoryId)
    setPage(1)
  }

  const handleSearch = (query: string) => {
    setSearchQuery(query)
    setPage(1)
  }

  const handleAdvancedFilters = (filters: any) => {
    // Handle advanced filters - this could be expanded based on needs
    console.log("Advanced filters:", filters)
  }

  const loadMore = () => {
    if (!loading && hasMore) {
      loadPosts(false)
    }
  }

  return (
    <div className="space-y-8">
      {/* Header */}
      <div className="space-y-4">
        <h1 className="text-4xl font-bold">Todos los Posts</h1>
        <p className="text-muted-foreground text-lg">Explora todos los artículos de la comunidad DevTalles</p>
      </div>

      {/* Filters */}
      <div className="space-y-6">
        <div className="flex flex-col md:flex-row gap-4 items-start md:items-center justify-between">
          <SearchPosts onSearch={handleSearch} />
          <AdvancedFilters categories={categories} onFiltersChange={handleAdvancedFilters} />
        </div>
        <Separator />
        <CategoryFilter
          categories={categories}
          selectedCategory={selectedCategory}
          onCategoryChange={handleCategoryChange}
        />
      </div>

      {/* Posts Grid */}
      <div className="space-y-8">
        {loading && posts.length === 0 ? (
          <PostsLoading />
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {posts.map((post) => (
                <PostCard
                  key={post.id}
                  id={post.id}
                  title={post.title}
                  content={post.content}
                  category_name={post.category_name}
                  category_id={post.category_id}
                  author_name={post.author_name}
                  author_username={post.author_username}
                  author_photo={post.author_photo}
                  likes_count={post.likes_count}
                  created_at={post.created_at}
                />
              ))}
            </div>

            {/* Load More Button */}
            {hasMore && (
              <div className="flex justify-center">
                <Button onClick={loadMore} disabled={loading} size="lg">
                  {loading ? "Cargando..." : "Cargar Más Posts"}
                </Button>
              </div>
            )}

            {posts.length === 0 && !loading && (
              <div className="text-center py-12">
                <p className="text-muted-foreground text-lg">No se encontraron posts con los filtros seleccionados.</p>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  )
}

export default function PostsPage() {
  return (
    <Suspense fallback={<PostsLoading />}>
      <PostsContent />
    </Suspense>
  )
}
