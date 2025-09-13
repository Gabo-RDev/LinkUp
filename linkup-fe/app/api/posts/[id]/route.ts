import { type NextRequest, NextResponse } from "next/server"
import { getPostById, getCommentsByPostId, getPostLikeCount } from "@/lib/database"

export const dynamic = "force-dynamic"

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params

    if (!id) {
      return NextResponse.json({ error: "Post ID is required" }, { status: 400 })
    }

    const [post, comments, likesCount] = await Promise.all([
      getPostById(id),
      getCommentsByPostId(id),
      getPostLikeCount(id),
    ])

    if (!post) {
      return NextResponse.json({ error: "Post not found" }, { status: 404 })
    }

    return NextResponse.json({
      post: {
        ...post,
        likes_count: likesCount,
      },
      comments,
    })
  } catch (error) {
    console.error("Error fetching post:", error)
    return NextResponse.json({ error: "Failed to fetch post" }, { status: 500 })
  }
}
