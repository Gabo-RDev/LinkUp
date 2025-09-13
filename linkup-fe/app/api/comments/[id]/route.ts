import { type NextRequest, NextResponse } from "next/server"
import { updateComment, deleteComment } from "@/lib/likes-comments"

export const dynamic = "force-dynamic"

export async function PUT(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: commentId } = params
    const body = await request.json()
    const { userId, description } = body

    if (!userId || !description?.trim()) {
      return NextResponse.json({ error: "User ID and description are required" }, { status: 400 })
    }

    const comment = await updateComment(commentId, userId, description.trim())

    if (!comment) {
      return NextResponse.json({ error: "Comment not found or unauthorized" }, { status: 404 })
    }

    return NextResponse.json({
      success: true,
      comment,
    })
  } catch (error) {
    console.error("Error updating comment:", error)
    return NextResponse.json({ error: "Failed to update comment" }, { status: 500 })
  }
}

export async function DELETE(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: commentId } = params
    const body = await request.json()
    const { userId } = body

    if (!userId) {
      return NextResponse.json({ error: "User ID is required" }, { status: 400 })
    }

    const comment = await deleteComment(commentId, userId)

    if (!comment) {
      return NextResponse.json({ error: "Comment not found or unauthorized" }, { status: 404 })
    }

    return NextResponse.json({
      success: true,
      message: "Comment deleted successfully",
    })
  } catch (error) {
    console.error("Error deleting comment:", error)
    return NextResponse.json({ error: "Failed to delete comment" }, { status: 500 })
  }
}
