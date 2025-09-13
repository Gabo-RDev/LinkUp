import { Suspense } from "react"
import { getPosts, getCategories } from "@/lib/database"
import { PostCard } from "@/components/post-card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { PostsLoading } from "@/components/posts-loading"
import { TrendingUp, Users, BookOpen, Code2, ArrowRight } from "lucide-react"
import Link from "next/link"

async function PostsGrid() {
  const posts = await getPosts(6) // Show only 6 posts on homepage

  return (
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
  )
}

async function CategoriesSection() {
  const categories = await getCategories()

  return (
    <div className="flex flex-wrap gap-2">
      {categories.slice(0, 8).map((category) => (
        <Link key={category.id} href={`/posts?category=${category.id}`}>
          <Badge
            variant="outline"
            className="cursor-pointer hover:bg-primary hover:text-primary-foreground transition-colors"
          >
            {category.name}
          </Badge>
        </Link>
      ))}
    </div>
  )
}

function PostsSkeleton() {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {Array.from({ length: 6 }).map((_, i) => (
        <div key={i} className="space-y-4">
          <div className="h-48 w-full bg-muted animate-pulse rounded-lg" />
          <div className="h-4 w-3/4 bg-muted animate-pulse" />
          <div className="h-4 w-1/2 bg-muted animate-pulse" />
        </div>
      ))}
    </div>
  )
}

export default function HomePage() {
  return (
    <div className="space-y-12">
      {/* Hero Section */}
      <section className="text-center space-y-6 py-12">
        <div className="space-y-4">
          <h1 className="text-4xl md:text-6xl font-bold text-balance">
            Bienvenido al <span className="text-primary">DevTalles Blog</span>
          </h1>
          <p className="text-xl text-muted-foreground max-w-2xl mx-auto text-pretty">
            Únete a nuestra comunidad de desarrolladores. Comparte conocimiento, aprende nuevas tecnologías y conecta
            con otros programadores.
          </p>
        </div>

        <div className="flex flex-wrap justify-center gap-4">
          <Button size="lg" className="gap-2" asChild>
            <Link href="/posts">
              <BookOpen className="h-4 w-4" />
              Explorar Posts
            </Link>
          </Button>
          <Button variant="outline" size="lg" className="gap-2 bg-transparent" asChild>
            <Link href="/auth/register">
              <Users className="h-4 w-4" />
              Únete a la Comunidad
            </Link>
          </Button>
        </div>
      </section>

      {/* Stats Section */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="text-center space-y-2 p-6 rounded-lg border bg-card">
          <TrendingUp className="h-8 w-8 mx-auto text-primary" />
          <h3 className="text-2xl font-bold">500+</h3>
          <p className="text-muted-foreground">Posts Publicados</p>
        </div>
        <div className="text-center space-y-2 p-6 rounded-lg border bg-card">
          <Users className="h-8 w-8 mx-auto text-primary" />
          <h3 className="text-2xl font-bold">1,200+</h3>
          <p className="text-muted-foreground">Miembros Activos</p>
        </div>
        <div className="text-center space-y-2 p-6 rounded-lg border bg-card">
          <Code2 className="h-8 w-8 mx-auto text-primary" />
          <h3 className="text-2xl font-bold">15+</h3>
          <p className="text-muted-foreground">Tecnologías</p>
        </div>
      </section>

      {/* Categories Section */}
      <section className="space-y-6">
        <div className="flex items-center justify-between">
          <h2 className="text-3xl font-bold">Categorías Populares</h2>
          <Button variant="outline" asChild>
            <Link href="/posts" className="gap-2">
              Ver Todas
              <ArrowRight className="h-4 w-4" />
            </Link>
          </Button>
        </div>
        <Suspense
          fallback={
            <div className="flex flex-wrap gap-2">
              {Array.from({ length: 8 }).map((_, i) => (
                <div key={i} className="h-6 w-20 bg-muted animate-pulse rounded" />
              ))}
            </div>
          }
        >
          <CategoriesSection />
        </Suspense>
      </section>

      {/* Latest Posts Section */}
      <section className="space-y-6">
        <div className="flex items-center justify-between">
          <h2 className="text-3xl font-bold">Últimos Posts</h2>
          <Button variant="outline" asChild>
            <Link href="/posts" className="gap-2">
              Ver Todos
              <ArrowRight className="h-4 w-4" />
            </Link>
          </Button>
        </div>
        <Suspense fallback={<PostsLoading />}>
          <PostsGrid />
        </Suspense>
      </section>

      {/* CTA Section */}
      <section className="text-center space-y-6 py-12 bg-muted/50 rounded-lg">
        <div className="space-y-4">
          <h2 className="text-3xl font-bold">¿Listo para compartir tu conocimiento?</h2>
          <p className="text-muted-foreground max-w-xl mx-auto">
            Únete a miles de desarrolladores que ya forman parte de la comunidad DevTalles
          </p>
        </div>
        <Button size="lg" className="gap-2" asChild>
          <Link href="/auth/register">
            Crear Cuenta
            <ArrowRight className="h-4 w-4" />
          </Link>
        </Button>
      </section>
    </div>
  )
}
