import { PostForm } from "@/components/admin/post-form"

export default function NewPostPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold">Crear Nuevo Post</h1>
        <p className="text-muted-foreground">Comparte conocimiento con la comunidad DevTalles</p>
      </div>

      <PostForm />
    </div>
  )
}
