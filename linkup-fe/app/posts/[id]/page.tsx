import { Suspense } from "react"
import { notFound } from "next/navigation"
import { PostContent } from "@/components/post-content"
import { CommentsSection } from "@/components/comments-section"
import { RelatedPosts } from "@/components/related-posts"
import { Skeleton } from "@/components/ui/skeleton"
import { Card, CardContent } from "@/components/ui/card"

async function getPostData(id: string) {
  try {
    const [postResponse, relatedResponse] = await Promise.all([
      fetch(`${process.env.NEXT_PUBLIC_BASE_URL || "http://localhost:3000"}/api/posts/${id}`, {
        cache: "no-store",
      }),
      fetch(`${process.env.NEXT_PUBLIC_BASE_URL || "http://localhost:3000"}/api/posts/${id}/related`, {
        cache: "no-store",
      }),
    ])

    if (!postResponse.ok) {
      if (postResponse.status === 404) {
        notFound()
      }
      throw new Error("Failed to fetch post")
    }

    const postData = await postResponse.json()
    const relatedData = await relatedResponse.json()

    return {
      post: postData.post,
      comments: postData.comments || [],
      relatedPosts: relatedData.relatedPosts || [],
    }
  } catch (error) {
    console.error("Error fetching post data:", error)
    throw error
  }
}

function PostSkeleton() {
  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <div className="space-y-6">
        <Skeleton className="h-6 w-20" />
        <Skeleton className="h-12 w-full" />
        <Skeleton className="h-12 w-3/4" />
        <div className="flex items-center space-x-4">
          <Skeleton className="h-12 w-12 rounded-full" />
          <div className="space-y-2">
            <Skeleton className="h-4 w-32" />
            <Skeleton className="h-3 w-24" />
          </div>
        </div>
      </div>
      <Card>
        <CardContent className="p-8 space-y-4">
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-3/4" />
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-2/3" />
        </CardContent>
      </Card>
    </div>
  )
}

export default async function PostPage({ params }: { params: { id: string } }) {
  return (
    <div className="space-y-12">
      <Suspense fallback={<PostSkeleton />}>
        <PostPageContent postId={params.id} />
      </Suspense>
    </div>
  )
}

async function PostPageContent({ postId }: { postId: string }) {
  const { post, comments, relatedPosts } = await getPostData(postId)

  return (
    <>
      <PostContent post={post} />
      <CommentsSection comments={comments} postId={postId} />
      <RelatedPosts posts={relatedPosts} />
    </>
  )
}

export async function generateMetadata({ params }: { params: { id: string } }) {
  try {
    const { post } = await getPostData(params.id)

    return {
      title: `${post.title} | DevTalles Blog`,
      description: post.content.substring(0, 160) + "...",
      openGraph: {
        title: post.title,
        description: post.content.substring(0, 160) + "...",
        type: "article",
        authors: [post.author_name],
      },
    }
  } catch {
    return {
      title: "Post no encontrado | DevTalles Blog",
    }
  }
}
